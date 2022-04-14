namespace API.DTOs;
public class NewTestGroupDto
{
    public int Number { get; set; }
    public int Points { get; set; }
    public int TimeLimit { get; set; }
    public ICollection<NewTestDto>? Tests { get; set; }
}