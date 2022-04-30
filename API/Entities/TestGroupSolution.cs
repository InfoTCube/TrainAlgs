using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TestGroupSolutions")]
public class TestGroupSolution
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Points { get; set; }
    public Solution? Solution { get; set; }
    public int SolutionId { get; set; }
    public ICollection<TestSolution>? Tests { get; set; }
}