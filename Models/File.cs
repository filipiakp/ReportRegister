using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Models
{
    public class File
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(400)]
        public string Name { get; set; }
        public override string ToString()
        {
            return Name.Substring(Name.IndexOf('_') + 1);
        }
    }
}
