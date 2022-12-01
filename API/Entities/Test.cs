using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Tests")]
public class Test
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string? Input { get; set; }
    public string? Output { get; set; }
    public TestGroup? TestGroup { get; set; }
    public int TestGroupId { get; set; }
}