namespace API.DTOs;
public class UserDto
{
    public string? Username { get; set; }
    public string? Token { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public ICollection<SolutionDto>? Solutions { get; set; }
}