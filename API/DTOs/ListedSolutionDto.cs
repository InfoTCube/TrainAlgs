namespace API.DTOs;
public class ListedSolutionDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Language { get; set; }
    public string? Status { get; set; }
    public int Points { get; set; }
    public string? AlgTaskTag { get; set; }
    public string? AuthorUsername { get; set; }
    public DateTime Date { get; set; }
    public ICollection<TestGroupSolutionDto>? TestGroups { get; set; }
}