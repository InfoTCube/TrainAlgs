using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;
public interface ITaskRepository
{
    void Update(AlgTask task);
    Task AddTaskAsync(AlgTask task);
    Task<AlgTask> GetTaskByIdAsync(int id);
    Task<AlgTask> GetTaskByNameTagAsync(string nameTag);
    Task<PagedList<ListedTaskDto>> GetTasksAsync(ElementParams elementParams);
}