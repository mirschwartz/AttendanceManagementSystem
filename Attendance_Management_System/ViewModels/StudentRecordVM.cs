using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.ViewModels
{
    public class StudentRecordVM
    {
        public Student Student { get; set; }

        public string Classes { get; set; }

        public List<Data.Models.Attendance> Attendance { get; set; }
    }
}