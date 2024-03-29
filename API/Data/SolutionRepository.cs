using System.Linq;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class SolutionRepository : ISolutionRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public SolutionRepository(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task AddSolutionAsync(Solution solution)
    {
        await _context.Solutions.AddAsync(solution);
    }

    public async Task<Solution> GetSolutionByIdAsync(int id)
    {
        var solution =  await _context.Solutions
            .Include(solution => solution.TestGroups)
            .Include("TestGroups.Tests")
            .Include(solution => solution.Task)
            .Include(solution => solution.Task.TestGroups)
            .Include("Task.TestGroups.Tests")
            .Include(solution => solution.Author)
            .Where(solution => solution.Id == id)
            .SingleOrDefaultAsync();

        return solution;
    }

    public async Task<PagedList<ListedSolutionDto>> GetSolutionsForUserAsync(string username, ElementParams elementParams)
    {
        var query = _context.Solutions
            .Where(s => s.Author.UserName == username)
            .OrderByDescending(d => d.Date)
            .ProjectTo<ListedSolutionDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();
        return await PagedList<ListedSolutionDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public async Task<PagedList<ListedSolutionDto>> GetSolutionsForUserForTaskAsync(string username, ElementParams elementParams, string taskNameTag)
    {
        var query = _context.Solutions
            .Where(s => s.Author.UserName == username)
            .Where(s => s.Task.NameTag == taskNameTag)
            .OrderByDescending(d => d.Date)
            .ProjectTo<ListedSolutionDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();
        return await PagedList<ListedSolutionDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public void Update(Solution solution)
    {
        _context.Entry(solution).State = EntityState.Modified;
    }
}