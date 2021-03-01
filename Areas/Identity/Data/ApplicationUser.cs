using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ReportRegister.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        [PersonalData]
        public string FirstName { get; set; }
        [MaxLength(90)]
        [PersonalData]
        public string LastName { get; set; }
        public bool EmailNotifications { get; set; }
    }
}
