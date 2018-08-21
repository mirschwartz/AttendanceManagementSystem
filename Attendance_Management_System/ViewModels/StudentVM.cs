using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.ViewModels
{
    public class StudentVM
    {
        public int StudentId { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public int TotalAbsences { get; set; }

        public int TotalLatenesses { get; set; }

        public double Percentage { get; set; }
    }
}