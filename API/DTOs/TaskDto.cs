namespace API.DTOs;
public class TaskDto
{
    public string? Name { get; set; }
    public string? NameTag { get; set; }
    public string? ContentUrl { get; set; }
    public string? AuthorUsername { get; set; }
    public int MemoryLimit { get; set; }
    public ICollection<TestGroupDto>? TestGroups { get; set; }
}