using API.DTOs;
using API.Entities;
using API.Helpers;
using static API.Helpers.TaskParams;

namespace API.Interfaces;
public interface ITaskRepository
{
    void Update(AlgTask task);
    Task AddTaskAsync(AlgTask task);
    Task RateTaskAsync(AppUser user, AlgTask task, int rating);
    Task<bool> CanRateTaskAsync(AppUser user, string nameTag);
    Task <Rating> GetRatingByTaskAndUser(AppUser user, AlgTask task);
    Task<AlgTask> GetTaskByIdAsync(int id);
    Task<AlgTask> GetTaskByNameTagAsync(string nameTag);
    Task<AlgTask> GetTaskToVerifyByNameTagAsync(string nameTag);
    Task<PagedList<ListedTaskDto>> GetTasksAsync(ElementParams elementParams);
    Task<PagedList<ListedTaskDto>> GetTasksForUserAsync(TaskParams taskParams, string username);
    Task<PagedList<ListedTaskDto>> GetTasksToVerifyAsync(ElementParams elementParams);
    Task<PagedList<ListedTaskDto>> GetTasksWithUserResultsAsync(PagedList<ListedTaskDto> tasks, string username);
}