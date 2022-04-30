using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("Solutions")]
public class Solution
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Language { get; set; }
    public string? Status { get; set; }
    public int Points { get; set; }
    public AlgTask? Task { get; set; }
    public int TaskId { get; set; }
    public AppUser? Author { get; set; }
    public int AuthorId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public ICollection<TestGroupSolution>? TestGroups { get; set; }
}