﻿using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.DTOs
{
    public class EmployeeDto
    {
        public string EmployeeName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int Position { get; set; }
        public string EmployeeLevelName { get; set; }
        public DateTime RegisterDate { get; set; }

        public int Citys { get; set; }
        public int Areas { get; set; }

        public string Address { get; set; }
    }
}
