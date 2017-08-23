using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        
        public string Title { get; set; }

        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [MaxLength(15)]
        public string HomePhone { get; set; }

        [MaxLength(15)]
        public string CellPhone { get; set; }

        public bool IsActive { get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}
