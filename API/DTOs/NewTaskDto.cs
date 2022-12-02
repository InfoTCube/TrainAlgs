using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
public class NewTaskDto
{
    [Required]
    [StringLength(50, ErrorMessage = "Task name must be between 3 and 50 characters", MinimumLength = 3)]
    public string? Name { get; set; }
    [Required]
    public string? Content { get; set; }
    [Required]
    public int TimeLimit { get; set; }
    [Required]
    public int MemoryLimit { get; set; }
    [Required]
    public ICollection<NewTestGroupDto>? TestGroups { get; set; }
}