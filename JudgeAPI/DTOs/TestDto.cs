namespace JudgeAPI.DTOs;

public class TestDto
{
    public int Number { get; set; }
    public int TimeLimit { get; set; }
    public string? Input { get; set; }
    public string? Output { get; set; }
    public TestGroupDto? TestGroup { get; set; }
}