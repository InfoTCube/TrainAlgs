using System.Security.Claims;
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

    public async Task<PagedList<ListedTaskDto>> GetTasksAsync(ElementParams elementParams)
    {
        var query = _context.Tasks
            .Where(task => task.Verified == true)
            .Include("Solutions.Author")
            .ProjectTo<ListedTaskDto>(_mapper.ConfigurationProvider);

        return await PagedList<ListedTaskDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }

    public async Task<PagedList<ListedTaskDto>> GetTasksForUserAsync(TaskParams taskParams, string username)
    {
        var query = _context.Tasks
            .Where(task => task.Verified == true)
            .Include("Solutions.Author");

        query = taskParams.TaskStatus switch 
        {
            Status.Default => query,
            Status.NotAttempted => query.Where(task => task.Solutions.Where(s => s.Author.UserName == username).Count() == 0),
            Status.Solved => query.Where(task => task.Solutions.Where(s => s.Author.UserName == username).Max(s => s.Points) == 100),
            Status.Attempted => query.Where(task => task.Solutions.Where(s => s.Author.UserName == username).Max(s => s.Points) >= 0 
                && task.Solutions.Where(s => s.Author.UserName == username).Max(s => s.Points) < 100)
        };

        return await PagedList<ListedTaskDto>.CreateAsync(query.ProjectTo<ListedTaskDto>(_mapper.ConfigurationProvider), taskParams.PageNumber, taskParams.PageSize);
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

    public async Task<PagedList<ListedTaskDto>> GetTasksWithUserResultsAsync(PagedList<ListedTaskDto> tasks, string username)
    {
        foreach(var task in tasks)
        {
            var userSolutions = _context.Solutions
                .Include(s => s.Author)
                .Include(s => s.Task)
                .Where(s => s.Author.UserName == username)
                .Where(s => s.Task.NameTag == task.NameTag);
            task.UserScore = userSolutions.Count() != 0 ? userSolutions.Max(s => s.Points) : -1;
        }

        return tasks;
    }

    public async Task RateTaskAsync(AppUser user, AlgTask task, int rating)
    {
        var rate = new Rating 
        {
            User = user,
            Task = task,
            Rate = rating
        };

        await _context.Ratings.AddAsync(rate);
    }

    public async Task<Rating> GetRatingByTaskAndUser(AppUser user, AlgTask task)
    {
        return await _context.Ratings
            .Where(r => r.User == user)
            .Where(r => r.Task == task)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CanRateTaskAsync(AppUser user, string nameTag)
    {
        var task = await _context.Tasks.Include("Solutions.Author").FirstOrDefaultAsync(task => task.NameTag == nameTag);
        var solved = task.Solutions.Where(s => s.Author == user).Any(s => s.Points == 100);
        var rating = _context.Ratings
            .Where(r => r.User == user)
            .Where(r => r.Task == task)
            .Any();

        if(solved && !rating) return true;

        return false;
    }
}