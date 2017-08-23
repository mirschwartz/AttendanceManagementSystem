using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        
        [Required]
        public string ClassName { get; set; }
        
        public string Year { get; set; }

        public bool IsActive { get; set; }

        public ICollection<StudentClass> StudentClasses { get; set; }
    }
}
