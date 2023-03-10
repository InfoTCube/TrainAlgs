using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class UsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    public UsersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return Ok(await _unitOfWork.UserRepository.GetMemberAsync(username));
    }

    [HttpGet("search/{searchText}")]
    public async Task<ActionResult<IEnumerable<SearchedMemberDto>>> SearchForUsers(string searchText, [FromQuery] ElementParams elementParams)
    {
        return Ok(await _unitOfWork.UserRepository.SearchForMemberAsync(elementParams, searchText));
    }
    
}