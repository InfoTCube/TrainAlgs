namespace API.DTOs
{
    public class MemberDto
    {
        public string? Username { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
    }
}