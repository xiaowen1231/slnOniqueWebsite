namespace prjOniqueWebsite.Models.DTOs
{
    public class FMemberEditDto
    {
        public int MemberId { get; set; }
        public string? PhotoPath { get; set; }
        public string Name { get; set; }
        public string? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Gender { get; set; }
        public string Citys { get; set; }
        public string Areas { get; set; }
        public string? Address { get; set; }
    }
}
