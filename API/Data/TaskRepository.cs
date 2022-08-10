using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using static API.Helpers.TaskParams;

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
        return await _context.Tasks.Where(task => task.Verified == true).Where(task => task.Id == id).FirstOrDefaultAsync();
    }

    public async Task<AlgTask> GetTaskByNameTagAsync(string nameTag)
    {
        var task =  await _context.Tasks
            .Include(task => task.TestGroups)
            .Include("TestGroups.Tests")
            .Where(task => task.NameTag == nameTag)
            .Where(task => task.Verified == true)
            .SingleOrDefaultAsync();

        return task;
    }

    public async Task<AlgTask> GetTaskToVerifyByNameTagAsync(string nameTag)
    {
        var task =  await _context.Tasks
            .Include(task => task.TestGroups)
            .Include("TestGroups.Tests")
            .Where(task => task.NameTag == nameTag)
            .Where(task => task.Verified == false)
            .SingleOrDefaultAsync();

        return task;
    }

    public async Task<PagedList<ListedTaskDto>> GetTasksAsync(string username, ElementParams elementParams)
    {
        var query = _context.Tasks
            .Where(task => task.Verified == true)
            .Include("Solutions.Author")
            .ProjectTo<ListedTaskDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();
    
        return await PagedList<ListedTaskDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public async Task<PagedList<ListedTaskDto>> GetTasksToVerifyAsync(ElementParams elementParams)
    {
        var query = _context.Tasks
            .Where(task => task.Verified == false)
            .ProjectTo<ListedTaskDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<ListedTaskDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public void Update(AlgTask task)
    {
        _context.Entry(task).State = EntityState.Modified;
    }
}