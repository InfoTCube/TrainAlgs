namespace JudgeAPI.DTOs;
public class TestGroupDto
{
    public int Number { get; set; }
    public int Points { get; set; }
    public int TimeLimit { get; set; }
    public ICollection<TestDto>? Tests { get; set; }
}