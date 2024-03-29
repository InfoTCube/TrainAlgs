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

        foreach(var s in solutions)
        {
            s.AlgTaskName = (await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(s.AlgTaskTag)).Name;
        }

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("forUser/{username}")]
    public async Task<ActionResult<IEnumerable<ListedSolutionDto>>> GetSolutionsForUser([FromQuery] ElementParams elementParams, string username)
    {
        var solutions = await _unitOfWork.SolutionRepository.GetSolutionsForUserAsync(username, elementParams);

        foreach(var s in solutions)
        {
            s.AlgTaskName = (await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(s.AlgTaskTag)).Name;
        }

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("forCurrentUserAndTask/{taskNameTag}")]
    public async Task<ActionResult<IEnumerable<ListedSolutionDto>>> GetSolutionsForCurrentUserForTask([FromQuery] ElementParams elementParams, string taskNameTag)
    {
        string username = User.GetUsername();

        var solutions = await _unitOfWork.SolutionRepository.GetSolutionsForUserForTaskAsync(username, elementParams, taskNameTag);

        foreach(var s in solutions)
        {
            s.AlgTaskName = (await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(s.AlgTaskTag)).Name;
        }

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("forUserAndTask/{username}/{taskNameTag}")]
    public async Task<ActionResult<IEnumerable<ListedSolutionDto>>> GetSolutionsForUserForTask([FromQuery] ElementParams elementParams, string username, string taskNameTag)
    {
        var solutions = await _unitOfWork.SolutionRepository.GetSolutionsForUserForTaskAsync(username, elementParams, taskNameTag);
        
        foreach(var s in solutions)
        {
            s.AlgTaskName = (await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(s.AlgTaskTag)).Name;
        }

        Response.AddPaginationHeader(solutions.CurrentPage, solutions.PageSize,
            solutions.TotalCount, solutions.TotalPages);

        return Ok(solutions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SolutionDto>> GetSolution(int id)
    {
        string username = User.GetUsername();
        var solution = await _unitOfWork.SolutionRepository.GetSolutionByIdAsync(id);
        if(solution is null) return NotFound();
        if(solution.Author.UserName != username) return Unauthorized();
        return _mapper.Map<SolutionDto>(solution);
    }

    [HttpPost]
    public async Task<ActionResult<NewSolutionDto>> AddSolution(NewSolutionDto solutionDto)
    {
        string username = User.GetUsername();
        var task = await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(solutionDto.AlgTaskTag);
        if(task == null) 
            return NotFound("This task doesn't exist...");

        Solution solution = new Solution
        {
            Language = solutionDto.Language,
            Code = solutionDto.Code,
            Author = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username),
            Task = task,
            Status = "Waiting for the results"
        };

        //await _unitOfWork.SolutionRepository.AddSolutionAsync(solution);
        //await _unitOfWork.Complete();

        //getting results
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);
        HttpResponseMessage response = new HttpResponseMessage();

        var taskToTest = _mapper.Map<AlgTaskToTestDto>(task);
        taskToTest.Code = solutionDto.Code;

        if(solution.Language == "C++")
            response = await client.PostAsJsonAsync("https://localhost:7279/judge/Cpp/TestTaskCpp", taskToTest);
        else if(solution.Language == "PYTHON")
            response = await client.PostAsJsonAsync("https://localhost:7279/judge/Python/TestTaskPython", taskToTest);
        
        if(!response.IsSuccessStatusCode)
        {
            solution.Status = "Judge error";
            await _unitOfWork.SolutionRepository.AddSolutionAsync(solution);
            //_unitOfWork.SolutionRepository.Update(solution);
            await _unitOfWork.Complete();
            return solutionDto;
        }

        Solution result = await response.Content.ReadFromJsonAsync<Solution>();

        solution.Points = result.Points;
        solution.Status = result.Status;
        solution.ErrorMessage = result.ErrorMessage;
        solution.TestGroups = result.TestGroups;

        //_unitOfWork.SolutionRepository.Update(solution);
        await _unitOfWork.SolutionRepository.AddSolutionAsync(solution);
        await _unitOfWork.Complete();

        return solutionDto;
    }
}