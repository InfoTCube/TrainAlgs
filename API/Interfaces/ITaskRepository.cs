using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ITaskRepository
    {
        void Update(AlgTask task);
        Task<AlgTask> GetTaskByIdAsync(int id);
        Task<TaskDto> GetTaskByNameTagAsync(string nameTag);
        Task<PagedList<TaskDto>> GetTasksAsync(ElementParams elementParams);
    }
}