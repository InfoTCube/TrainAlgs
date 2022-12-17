using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class TasksController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public TasksController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListedTaskDto>>> GetTasks([FromQuery] TaskParams elementParams)
    {
        var tasks = await _unitOfWork.TaskRepository.GetTasksAsync(elementParams);
        tasks = await _unitOfWork.TaskRepository.GetTasksWithUserResultsAsync(tasks, User.GetUsername());

        Response.AddPaginationHeader(tasks.CurrentPage, tasks.PageSize,
            tasks.TotalCount, tasks.TotalPages);

        return Ok(tasks);
    }

    [HttpGet("{nameTag}")]
    public async Task<ActionResult<TaskDto>> GetTask(string nameTag)
    {
        var task = await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(nameTag);
        return _mapper.Map<TaskDto>(task);
    }

    [Authorize]
    [HttpGet("ForCurrentUser")]
    public async Task<ActionResult<IEnumerable<ListedTaskDto>>> GetTasksForUser([FromQuery] TaskParams taskParams)
    {
        var tasks = await _unitOfWork.TaskRepository.GetTasksForUserAsync(taskParams, User.GetUsername());
        tasks = await _unitOfWork.TaskRepository.GetTasksWithUserResultsAsync(tasks, User.GetUsername());

        Response.AddPaginationHeader(tasks.CurrentPage, tasks.PageSize,
            tasks.TotalCount, tasks.TotalPages);

        return Ok(tasks);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<NewTaskDto>> AddTask(NewTaskDto taskDto)
    {
        string nametag = await GenerateNametag(taskDto.Name);

        AlgTask task = _mapper.Map<AlgTask>(taskDto);
        task.NameTag = nametag;
        string username = User.GetUsername();
        task.Author = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        await _unitOfWork.TaskRepository.AddTaskAsync(task);
        await _unitOfWork.Complete();

        return taskDto;
    }

    private async Task<bool> TaskExists(string nameTag)
    {
        return (await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(nameTag) is not null);
    }

    private async Task<string> GenerateNametag(string name)
    {
        name = name.Substring(0, 3).ToUpper();
        string nametag = name;
        int counter = 1;
        while(await TaskExists(nametag))
        {
            nametag = $"{name}{counter}";
            ++counter;
        }

        return nametag;
    }
}