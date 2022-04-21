using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
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
    public async Task<ActionResult<IEnumerable<ListedTaskDto>>> GetTasks([FromQuery] ElementParams elementParams)
    {
        var tasks = await _unitOfWork.TaskRepository.GetTasksAsync(elementParams);

        Response.AddPaginationHeader(tasks.CurrentPage, tasks.PageSize,
            tasks.TotalCount, tasks.TotalPages);

        return Ok(tasks);
    }

    [HttpGet("{nameTag}")]
    public async Task<ActionResult<TaskDto>> GetTask(string nameTag)
    {
        return await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(nameTag);
    }

    [HttpPost]
    public async Task<ActionResult<NewTaskDto>> AddTask(NewTaskDto taskDto)
    {
        if(await TaskExists(taskDto.NameTag)) return BadRequest("Task with the same name tag already exist");

        AlgTask task = _mapper.Map<AlgTask>(taskDto);
        task.Author = await _unitOfWork.UserRepository.GetUserByUsernameAsync("info");

        await _unitOfWork.TaskRepository.AddTaskAsync(task);
        await _unitOfWork.Complete();

        return taskDto;
    }

    private async Task<bool> TaskExists(string nameTag)
    {
        return (await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(nameTag) is not null);
    }
}