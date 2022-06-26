namespace JudgeAPI.DTOs;
public class SolutionDto
{
    public string? Status { get; set; }
    public int Points { get; set; }
    public ICollection<TestGroupSolutionDto>? TestGroups { get; set; }
}