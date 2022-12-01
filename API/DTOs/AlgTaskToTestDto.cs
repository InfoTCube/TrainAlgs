using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public class AlgTaskToTestDto
{
    public string? Code { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public ICollection<TestGroupToTestDto>? TestGroups { get; set; }
}
