using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
