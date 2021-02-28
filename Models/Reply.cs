using ReportRegister.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Models
{
    public class Reply
    {
        public long Id { get; set; }
        [MaxLength(4000)]
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public ApplicationUser Author { get; set; }
    }
}
