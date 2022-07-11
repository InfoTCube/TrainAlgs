using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TestGroups")]
public class TestGroup
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Points { get; set; }
    public AlgTask? AlgTask { get; set; }
    public int AlgTaskId { get; set; }
    public ICollection<Test>? Tests { get; set; }
}