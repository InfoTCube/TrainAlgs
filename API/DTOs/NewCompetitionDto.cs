using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;
public class NewCompetitionDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<NewTaskDto>? Tasks { get; set; }
    public int SubmissionsLimit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}