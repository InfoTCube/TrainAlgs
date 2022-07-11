namespace API.DTOs;
public class TaskDto
{
    public string? Name { get; set; }
    public string? NameTag { get; set; }
    public string? Content { get; set; }
    public string? AuthorUsername { get; set; }
    public TestGroupDto? ExampleTestGroup { get; set; }
    public ICollection<SolutionDto>? Solutions { get; set; }
}