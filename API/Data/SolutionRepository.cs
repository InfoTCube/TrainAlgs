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
        return await _context.Solutions.FindAsync(id);
    }

    public async Task<PagedList<ListedSolutionDto>> GetSolutionsForUserAsync(string username, ElementParams elementParams)
    {
        var query = _context.Solutions
            .Where(s => s.Author.UserName == username)
            .ProjectTo<ListedSolutionDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();
        return await PagedList<ListedSolutionDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public async Task<PagedList<ListedSolutionDto>> GetSolutionsForUserForTaskAsync(string username, ElementParams elementParams, string taskNameTag)
    {
        var query = _context.Solutions
            .Where(s => s.Author.UserName == username)
            .Where(s => s.Task.NameTag == taskNameTag)
            .ProjectTo<ListedSolutionDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();
        return await PagedList<ListedSolutionDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }
}