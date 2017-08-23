using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public TeacherSubject TeacherSubject { get; set; }

        public int TeacherSubjectId { get; set; }

        public StudentClass StudentClass { get; set; }

        public int StudentClassId { get; set; }

        public DateTime Date { get; set; }

        public int Period { get; set; }

        public Status Status { get; set; }
        
        public bool? SigningIn { get; set; }

        public string notes { get; set; }
    }
}
