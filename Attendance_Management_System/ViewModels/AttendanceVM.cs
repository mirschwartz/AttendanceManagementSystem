using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.ViewModels
{
    public class AttendanceVM
    {
        public List<Class> Classes { get; set; }

        public List<TeacherSubject> Subjects { get; set; }
    }
}