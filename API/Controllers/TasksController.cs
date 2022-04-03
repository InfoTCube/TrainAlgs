using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TasksController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public TasksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery]ElementParams elementParams)
        {
            var tasks = await _unitOfWork.TaskRepository.GetTasksAsync(elementParams);

            Response.AddPaginationHeader(tasks.CurrentPage, tasks.PageSize, 
                tasks.TotalCount, tasks.TotalPages);

            return Ok(tasks);
        }

        [HttpGet("/{nameTag}")]
        public async Task<ActionResult<TaskDto>> GetTask(string nameTag)
        {
            return await _unitOfWork.TaskRepository.GetTaskByNameTagAsync(nameTag);
        }
    }
}