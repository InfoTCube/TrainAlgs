using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TestSolutions")]
public class TestSolution
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Time { get; set; }
    public string? Status { get; set; }
    public string? Error { get; set; }
    public TestGroupSolution? TestGroup { get; set; }
    public int TestGroupId { get; set; }
}