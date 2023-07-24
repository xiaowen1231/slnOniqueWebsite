using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class EditMemberVM
    {
        [Required]
        public int MemberId { get; set; }
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
