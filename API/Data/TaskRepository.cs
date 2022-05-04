using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class TaskRepository : ITaskRepository
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    public TaskRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddTaskAsync(AlgTask task)
    {
        await _context.Tasks.AddAsync(task);
    }

    public async Task<AlgTask> GetTaskByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<AlgTask> GetTaskByNameTagAsync(string nameTag)
    {
        var task =  await _context.Tasks
            .Include(task => task.TestGroups)
            .Include("TestGroups.Tests")
            .Where(task => task.NameTag == nameTag)
            .SingleOrDefaultAsync();

        return task;
    }

    public async Task<PagedList<ListedTaskDto>> GetTasksAsync(ElementParams elementParams)
    {
        var query = _context.Tasks
            .ProjectTo<ListedTaskDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<ListedTaskDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public void Update(AlgTask task)
    {
        _context.Entry(task).State = EntityState.Modified;
    }
}