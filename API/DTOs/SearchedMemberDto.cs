namespace API.DTOs;
public class SearchedMemberDto
{
    public string? Username { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public string? Country { get; set; }
    public bool isModerator { get; set; }
    public bool isAdmin { get; set; }
}