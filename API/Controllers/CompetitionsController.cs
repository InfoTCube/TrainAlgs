using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CompetitionsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public CompetitionsController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListedCompetitionDto>>> GetCompetitions([FromQuery] ElementParams compParams)
    {
        var comps = await _unitOfWork.CompetitionRepository.GetCompetitionsAsync(compParams);

        Response.AddPaginationHeader(comps.CurrentPage, comps.PageSize,
            comps.TotalCount, comps.TotalPages);

        return Ok(comps);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<CompetitionDto>> GetCompetition(int id)
    {
        var comp = await _unitOfWork.CompetitionRepository.GetCompetitionByIdAsync(id);
        Console.WriteLine(comp.Tasks.FirstOrDefault().Name);
        return Ok(_mapper.Map<CompetitionDto>(comp));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateCompetition(NewCompetitionDto newCompetitionDto)
    {
        Competition competition = _mapper.Map<Competition>(newCompetitionDto);
        string username = User.GetUsername();
        AppUser author = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        competition.Organizer = author;
        foreach(var task in competition.Tasks)
        {
            task.Author = author;
            string nametag = await GenerateNametag(task.Name);
            task.NameTag = nametag;
        }
        await _unitOfWork.CompetitionRepository.AddCompetitionAsync(competition);
        await _unitOfWork.Complete();

        return Ok();
    }

    private async Task<bool> TaskExists(string nameTag)
    {
        return await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(nameTag) is not null;
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