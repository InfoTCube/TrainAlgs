namespace API.Entities;

public class Competition
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<AlgTask>? Tasks { get; set; }
    public int SubmissionsLimit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<AppUserCompetition>? Participants { get; set; }
    public bool Verified { get; set; } = false;
    public AppUser? Organizer { get; set; }
}