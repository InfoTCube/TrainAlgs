using static API.Helpers.TaskParams;

namespace API.DTOs;
public class ListedTaskDto
{
    public string? Name { get; set; }
    public string? NameTag { get; set; }
    public string? AuthorUsername { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public int Submissions { get; set; } = 0;
    public short CorrectPercent { get; set; } = 0;
    public short AverageResult { get; set; } = 0;
    public double Rating { get; set; }
    public int UserScore { get; set; }
}