using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public class TestGroupToTestDto
{
    public int Number { get; set; }
    public int Points { get; set; }
    public ICollection<TestToTestDto>? Tests { get; set; }
}
