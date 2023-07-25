using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.DTOs
{
    public class MemberDto
    {
        public int MemberId { get; set; }
        [Required]
        
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Citys { get; set; }
        public string Areas { get; set; }
        public string Address { get; set; }
        public string MemberLevel { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
