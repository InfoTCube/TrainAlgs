using JudgeAPI.DTOs;
using JudgeAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JudgeAPI.Controllers;
public class CppController : BaseApiController
{
    private readonly ICppService _cppService;
    public CppController(ICppService cppService)
    {
        _cppService = cppService;
    }

    [HttpPost("TestTaskCpp")]
    public async Task<ActionResult<SolutionDto>> TestTaskCpp(AlgTaskDto algTask)
    {
        SolutionDto solution = await _cppService.RunCpp(algTask);
        return solution;
    }
}