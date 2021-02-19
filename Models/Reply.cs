using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Models
{
    public class Reply
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
    }
}
