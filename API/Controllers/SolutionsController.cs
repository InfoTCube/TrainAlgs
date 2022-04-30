using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class SolutionsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SolutionsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("forCurrentUser")]
    public async Task<ActionResult<IEnumerable<ListedSolutionDto>>> GetSolutionsForCurrentUser([FromQuery] ElementParams elementParams)
    {
        string username = User.GetUsername();

        var solutions = await _unitOfWork.SolutionRepository.GetSolutionsForUserAsync(username, elementParams);

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("forUser/{username}")]
    public async Task<ActionResult<IEnumerable<ListedSolutionDto>>> GetSolutionsForUser([FromQuery] ElementParams elementParams, string username)
    {
        var solutions = await _unitOfWork.SolutionRepository.GetSolutionsForUserAsync(username, elementParams);

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("forCurrentUserandTask/{taskNameTag}")]
    public async Task<ActionResult<IEnumerable<ListedSolutionDto>>> GetSolutionsForCurrentUserForTask([FromQuery] ElementParams elementParams, string taskNameTag)
    {
        string username = User.GetUsername();

        var solutions = await _unitOfWork.SolutionRepository.GetSolutionsForUserForTaskAsync(username, elementParams, taskNameTag);

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("forUserandTask/{username}/{taskNameTag}")]
    public async Task<ActionResult<IEnumerable<ListedSolutionDto>>> GetSolutionsForUserForTask([FromQuery] ElementParams elementParams, string username, string taskNameTag)
    {
        var solutions = await _unitOfWork.SolutionRepository.GetSolutionsForUserForTaskAsync(username, elementParams, taskNameTag);

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SolutionDto>> GetSolution(int id)
    {
        var solution = await _unitOfWork.SolutionRepository.GetSolutionByIdAsync(id);
        if(solution is null) return NotFound();
        return _mapper.Map<SolutionDto>(solution);
    }

    [HttpPost]
    public async Task<ActionResult<NewSolutionDto>> AddSolution(NewSolutionDto solutionDto)
    {
        string username = User.GetUsername();
        Solution solution = new Solution
        {
            Language = solutionDto.Language,
            Code = solutionDto.Code,
            Author = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username),
            Task = await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(solutionDto.AlgTaskTag)
        };

        await _unitOfWork.SolutionRepository.AddSolutionAsync(solution);
        await _unitOfWork.Complete();

        return solutionDto;
    }
}