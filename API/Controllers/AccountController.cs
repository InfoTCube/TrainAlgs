using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _mapper = mapper;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        var user = _mapper.Map<AppUser>(registerDto);
        user.UserName = registerDto.Username.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");
        if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(user => user.UserName == loginDto.Username.ToLower());

        if(user == null) return Unauthorized("Invalid username");

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);

        if(!result.Succeeded) return Unauthorized();

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(name => name.UserName == username.ToLower());
    }
}