using Microsoft.AspNetCore.Http;
using ReportRegister.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Models
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(80)]
        public string Title { get; set; }
        [MaxLength(4000)]
        [Required]
        public string Description { get; set; }
        [Required]
        public ReportStatus Status { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<Reply> Replies { get; set; }
        public ApplicationUser Author { get; set; }
    }
}
