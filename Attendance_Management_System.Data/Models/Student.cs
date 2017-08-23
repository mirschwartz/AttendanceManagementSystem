using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [MaxLength(10)]
        public string Zip { get; set; }

        [MaxLength(15)]
        public string HomePhone { get; set; }

        [MaxLength(15)]
        public string CellPhone { get; set; }

        public bool IsActive { get; set; }

        public ICollection<StudentClass> StudentClasses { get; set; }
    }
}