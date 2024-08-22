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

[Authorize(Policy = "RequireModeratorRole")]
public class ModeratorController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ModeratorController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("verifyTasks")]
    public async Task<ActionResult<IEnumerable<ListedTaskDto>>> GetTasksToVerify([FromQuery] ElementParams elementParams)
    {
        var tasks = await _unitOfWork.TaskRepository.GetTasksToVerifyAsync(elementParams);

        Response.AddPaginationHeader(tasks.CurrentPage, tasks.PageSize,
            tasks.TotalCount, tasks.TotalPages);

        return Ok(tasks);
    }

    [HttpGet("verifyCompetitions")]
    public async Task<ActionResult<IEnumerable<ListedTaskDto>>> GetCompetitionsToVerify([FromQuery] ElementParams elementParams)
    {
        var comps = await _unitOfWork.CompetitionRepository.GetCompetitionsToVerifyAsync(elementParams);

        Response.AddPaginationHeader(comps.CurrentPage, comps.PageSize,
            comps.TotalCount, comps.TotalPages);

        return Ok(comps);
    }

    [HttpGet("verifyTasks/{nameTag}")]
    public async Task<ActionResult<TaskDto>> GetTaskToVerify(string nameTag)
    {
        var task = await _unitOfWork.TaskRepository.GetTaskToVerifyByNameTagAsync(nameTag);
        return _mapper.Map<TaskDto>(task);
    }

    [HttpGet("verifyCompetitions/{id}")]
    public async Task<ActionResult<CompetitionDto>> GetCompetitionToVerify(int id)
    {
        var comp = await _unitOfWork.CompetitionRepository.GetCompetitionToVerifyByIdAsync(id);
        return _mapper.Map<CompetitionDto>(comp);
    }

    [HttpPut("verifyTasks/{nameTag}")]
    public async Task<ActionResult> VerifyTask(string nameTag)
    {
        AlgTask task = await _unitOfWork.TaskRepository.GetTaskToVerifyByNameTagAsync(nameTag);
        if(task is null) return NotFound("Task with this name tag does not exist");
        if(task.Verified == true) return BadRequest("Task is already verified");

        task.Verified = true;
        _unitOfWork.TaskRepository.Update(task);

        if (await _unitOfWork.Complete()) return NoContent();

        return BadRequest("Failed to verify task");
    }

    [HttpPut("verifyCompetitions/{id}")]
    public async Task<ActionResult> VerifyCompetition(int id)
    {
        Competition comp = await _unitOfWork.CompetitionRepository.GetCompetitionToVerifyByIdAsync(id);
        if(comp is null) return NotFound("Competition with this name tag does not exist");
        if(comp.Verified == true) return BadRequest("Competition is already verified");

        comp.Verified = true;
        _unitOfWork.CompetitionRepository.Update(comp);

        if (await _unitOfWork.Complete()) return NoContent();

        return BadRequest("Failed to verify competition");
    }
}