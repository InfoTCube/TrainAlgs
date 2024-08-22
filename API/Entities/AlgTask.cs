using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AlgTasks")]
public class AlgTask
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? NameTag { get; set; }
    public string? Content { get; set; }
    public bool Verified { get; set; } = false;
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public AppUser? Author { get; set; }
    public int AuthorId { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
    public ICollection<TestGroup>? TestGroups { get; set; }
    public ICollection<Solution>? Solutions { get; set; }
    public Competition? Competition { get; set; }
}