using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.ViewModels
{
    public class TeacherVM
    {
        public int TeacherId { get; set; }

        public string Teacher { get; set; }

        public string Subjects { get; set; }

        public int Absentees { get; set; }

        public double Percentage { get; set; }
    }
}