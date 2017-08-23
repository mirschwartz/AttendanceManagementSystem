﻿using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.ViewModels
{
    public class ReportsVM
    {
        public List<Student> Students { get; set; }

        public List<Teacher> Teachers { get; set; }

        public List<Class> Classes { get; set; }

        public List<Attendance> Attendances { get; set; }

        public int ClassId { get; set; }

        public int Student { get; set; }

        public int Teacher { get; set; }

        public DateTime Date { get; set; }

        public Status Status { get; set; }
    }
}