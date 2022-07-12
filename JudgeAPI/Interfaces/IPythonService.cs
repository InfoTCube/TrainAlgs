
using JudgeAPI.DTOs;

namespace JudgeAPI.Interfaces;

public interface IPythonService
{
    Task<SolutionDto> RunPython(AlgTaskDto algTask);
}
