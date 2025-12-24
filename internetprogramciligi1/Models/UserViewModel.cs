using System.Collections.Generic;

namespace internetprogramciligi1.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; } // Kullanıcının rolleri (Admin, Uye vb.)
    }
}