﻿using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.ViewModels
{
    public class AdminStudentsVM
    {
        public List<BCClass> Classes { get; set; }

        public List<BCStudent> Students { get; set; }
    }
}