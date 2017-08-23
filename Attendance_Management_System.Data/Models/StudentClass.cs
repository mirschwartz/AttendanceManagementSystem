using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public class StudentClass
    {
        public int StudentClassId { get; set; }

        public Student Student { get; set; }

        public int StudentId { get; set; }

        public Class Class { get; set; }

        public int ClassId { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
    }
}