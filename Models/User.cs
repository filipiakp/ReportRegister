using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerificated { get; set; }
        public string Password { get; set; }
    }
}
