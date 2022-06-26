namespace JudgeAPI.DTOs;

public class TestGroupSolutionDto
{
    public int Number { get; set; }
    public int Points { get; set; }
    public ICollection<TestSolutionDto>? Tests { get; set; }
}