using System;
using System.Collections.Generic;

namespace DTO.DTO
{
    public class UserSgqDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Nullable<System.DateTime> AcessDate { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> ParCompany_Id { get; set; }
        public IEnumerable<int> ListParCompany_Id { get; set; }
        public IEnumerable<string> ListRole { get; set; }
        public bool IsActive { get; set; }
        public bool UseActiveDirectory { get; set; }
    }
}
