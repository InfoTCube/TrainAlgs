using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface ICompetitionRepository
{
    void Update(Competition comp);
    Task AddCompetitionAsync(Competition competition);
    Task<Competition> GetCompetitionByIdAsync(int id);
    Task<Competition> GetCompetitionToVerifyByIdAsync(int id);
    Task<PagedList<ListedCompetitionDto>> GetCompetitionsAsync(ElementParams compParams);
    Task<PagedList<ListedCompetitionDto>> GetCompetitionsToVerifyAsync(ElementParams elementParams);
}