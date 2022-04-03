using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("AlgTasks")]
    public class AlgTask
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameTag { get; set; }
        public string? ContentUrl { get; set; }
        public AppUser? Author { get; set; }
        public int AuthorId { get; set; }
    }
}