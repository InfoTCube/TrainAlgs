using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("assignModerator/{username}")]
        public async Task<ActionResult> AssignModerator(string username)
        {
            AppUser user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if(user is null) return NotFound("This user doesn't exist!");
            if((await _userManager.GetRolesAsync(user)).Any(role => role == "Moderator")) return BadRequest("User is already a moderator");

            var results = await _userManager.AddToRoleAsync(user, "Moderator");

            if (results.Succeeded) return NoContent();

            return BadRequest("Failed to assign user as a moderator");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("deleteUser/{username}")]
        public async Task<ActionResult> DeleteMember(string username)
        {
            AppUser user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if(user is null) return NotFound("This user doesn't exist!");
            if((await _userManager.GetRolesAsync(user)).Any(role => role == "Admin")) return Unauthorized();

            _unitOfWork.UserRepository.DeleteUser(user);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting the user");
        }
    }
}