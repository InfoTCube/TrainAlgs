using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;
public interface ISolutionRepository
{
    Task AddSolutionAsync(Solution solution);
    Task<Solution> GetSolutionByIdAsync(int id);
    Task<PagedList<ListedSolutionDto>> GetSolutionsForUserAsync(string username, ElementParams elementParams);
    Task<PagedList<ListedSolutionDto>> GetSolutionsForUserForTaskAsync(string username, ElementParams elementParams, string taskNameTag);
}