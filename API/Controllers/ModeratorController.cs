using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class ModeratorController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ModeratorController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [Authorize(Policy = "RequireModeratorRole")]
    [HttpGet("verify-tasks")]
    public async Task<ActionResult<IEnumerable<ListedTaskDto>>> GetTasksToVerify([FromQuery] ElementParams elementParams)
    {
        var tasks = await _unitOfWork.TaskRepository.GetTasksToVerifyAsync(elementParams);

        Response.AddPaginationHeader(tasks.CurrentPage, tasks.PageSize,
            tasks.TotalCount, tasks.TotalPages);

        return Ok(tasks);
    }

    [Authorize(Policy = "RequireModeratorRole")]
    [HttpGet("verify-tasks/{nameTag}")]
    public async Task<ActionResult<TaskDto>> GetTaskToVerify(string nameTag)
    {
        var task = await _unitOfWork.TaskRepository.GetTaskToVerifyByNameTagAsync(nameTag);
        return _mapper.Map<TaskDto>(task);
    }

    [Authorize(Policy = "RequireModeratorRole")]
    [HttpPut("verify-tasks/{nameTag}")]
    public async Task<ActionResult> VerifyTask(string nameTag)
    {
        AlgTask task = await _unitOfWork.TaskRepository.GetTaskToVerifyByNameTagAsync(nameTag);
        if(task is null) return NotFound("Task with this name tag does not exist");
        if(task.Verified == true) return BadRequest("Task is already verified");

        task.Verified = true;
        _unitOfWork.TaskRepository.Update(task);

        if (await _unitOfWork.Complete()) return NoContent();

        return BadRequest("Failed to update user");
    }
}