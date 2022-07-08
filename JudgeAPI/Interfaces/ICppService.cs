using JudgeAPI.DTOs;

namespace JudgeAPI.Interfaces;
public interface ICppService
{
    Task<SolutionDto> RunCpp(AlgTaskDto algTask);
}