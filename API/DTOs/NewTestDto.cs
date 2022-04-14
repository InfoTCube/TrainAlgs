namespace API.DTOs;
public class NewTestDto
{
    public int Number { get; set; }
    public int TimeLimit { get; set; }
    public string? Input { get; set; }
    public string? Output { get; set; }
}