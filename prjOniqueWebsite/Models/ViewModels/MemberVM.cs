using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class MemberVM
    {
        [Required]
        public int MemberId { get; set; }
        public string PhotoPath { get; set; }
        public IFormFile Photo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Citys { get; set; }
        public string Areas { get; set; }
        public string Address { get; set; }
        public string MemberLevel { get; set; }
        public string RegisterDate { get; set; }
        public string DateOfBirth { get; set; }
        
    }
}
