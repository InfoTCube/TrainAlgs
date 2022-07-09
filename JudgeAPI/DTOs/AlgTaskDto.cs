namespace JudgeAPI.DTOs;
public class AlgTaskDto
{
    public string? Code { get; set; }
    public ICollection<TestGroupDto>? TestGroups { get; set; }
}