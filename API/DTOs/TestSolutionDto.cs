namespace API.DTOs;
public class TestSolutionDto
{
    public int Number { get; set; }
    public int Time { get; set; }
    public int Memory { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public string? Status { get; set; }
    public string? Error { get; set; }
}