using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class UsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return Ok(await _unitOfWork.UserRepository.GetMemberAsync(username));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        _mapper.Map(memberUpdateDto, user);

        _unitOfWork.UserRepository.Update(user);

        if (await _unitOfWork.Complete()) return NoContent();

        return BadRequest("Failed to update user");
    }

    [HttpGet("search/{searchText}")]
    public async Task<ActionResult<IEnumerable<SearchedMemberDto>>> SearchForUsers(string searchText, [FromQuery] ElementParams elementParams)
    {
        return Ok(await _unitOfWork.UserRepository.SearchForMemberAsync(elementParams, searchText));
    }
    
}