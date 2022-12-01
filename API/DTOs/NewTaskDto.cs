using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
public class NewTaskDto
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Content { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public ICollection<NewTestGroupDto>? TestGroups { get; set; }
}