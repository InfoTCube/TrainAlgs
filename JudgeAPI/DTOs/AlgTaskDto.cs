namespace JudgeAPI.DTOs;
public class AlgTaskDto
{
    public string? Code { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public ICollection<TestGroupDto>? TestGroups { get; set; }
}