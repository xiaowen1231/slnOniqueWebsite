namespace prjOniqueWebsite.Models.DTOs
{
    public class MemberListDto
    {
        public int MemberId { get; set; }
        public string PhotoPath { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MemberLevelName { get; set; }
    }
}
