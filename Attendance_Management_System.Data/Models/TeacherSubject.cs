using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public class TeacherSubject
    {
        public int TeacherSubjectId { get; set; }

        public Teacher Teacher { get; set; }

        public int TeacherId { get; set; }

        [Required]
        public string Subject { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
    }
}
