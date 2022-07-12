using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JudgeAPI.DTOs;
using JudgeAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JudgeAPI.Controllers;

public class PythonController : BaseApiController
{
    private readonly IPythonService _pythonService;
    public PythonController(IPythonService pythonService)
    {
        _pythonService = pythonService;
    }

    [HttpPost("TestTaskPython")]
    public async Task<ActionResult<SolutionDto>> TestTaskCpp(AlgTaskDto algTask)
    {
        SolutionDto solution = await _pythonService.RunPython(algTask);
        return solution;
    }
}
