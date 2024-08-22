using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class CompetitionRepository : ICompetitionRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public CompetitionRepository(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task AddCompetitionAsync(Competition competition)
    {
        await _context.Competitions.AddAsync(competition);
    }

    public async Task<PagedList<ListedCompetitionDto>> GetCompetitionsToVerifyAsync(ElementParams elementParams)
    {
        var query = _context.Competitions
            .Where(comp => comp.Verified == false)
            .ProjectTo<ListedCompetitionDto>(_mapper.ConfigurationProvider);

        return await PagedList<ListedCompetitionDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public async Task<Competition> GetCompetitionByIdAsync(int id)
    {
        return await _context.Competitions
            .Include(comp => comp.Tasks)
            .Include("Tasks.Author")
            .Include("Tasks.TestGroups.Tests")
            .Include("Tasks.Solutions")
            .Include(comp => comp.Organizer)
            .Where(comp => comp.Verified == true)
            .Where(comp => comp.Id == id).FirstOrDefaultAsync();
    }

    public async Task<PagedList<ListedCompetitionDto>> GetCompetitionsAsync(ElementParams compParams)
    {
        var query = _context.Competitions
            .Where(comp => comp.Verified == true)
            .ProjectTo<ListedCompetitionDto>(_mapper.ConfigurationProvider);

        return await PagedList<ListedCompetitionDto>.CreateAsync(query, compParams.PageNumber, compParams.PageSize);
    }

    public async Task<Competition> GetCompetitionToVerifyByIdAsync(int id)
    {
        var comp =  await _context.Competitions
            .Include(comp => comp.Tasks)
            .Include("Tasks.Author")
            .Include("Tasks.TestGroups.Tests")
            .Where(comp => comp.Id == id)
            .Where(task => task.Verified == false)
            .SingleOrDefaultAsync();

        return comp;
    }

    public void Update(Competition comp)
    {
        _context.Entry(comp).State = EntityState.Modified;
    }
}