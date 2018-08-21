using Attendance_Management_System.Data.Models;
using Attendance_Management_System.Data.Repositories;
using Attendance_Management_System.Helpers;
using Attendance_Management_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance_Management_System.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ReportsRepository _ReportsRepo;

        public ReportsController()
        {
            _ReportsRepo = new ReportsRepository(ConnectionString.DB);
        }

        // GET: Reports
        public ActionResult Index(int? classId, int? student, int? teacher, DateTime? date, Status? status)
        {
            ReportsVM vm = new ReportsVM();
            vm.Teachers = _ReportsRepo.GetAllTeachers();
            vm.Students = _ReportsRepo.GetAllStudents();
            vm.Classes = _ReportsRepo.GetAllClasses();
            vm.Attendances = new List<BCAttendance>();

            if(classId.HasValue || student.HasValue || teacher.HasValue || date.HasValue || status.HasValue){
                vm.Attendances = _ReportsRepo.GetReport(classId, student, teacher, date, status).OrderBy(a => a.Date).OrderBy(a => a.StudentClass.Student.FirstName).ToList();
                vm.ClassId = classId.Value;
                vm.Student = student.Value;
                vm.Teacher = teacher.Value;
                vm.Status = status.Value;
                if(date != null)
                {
                    vm.Date = date.Value;
                }
            }
            return View(vm);
        }
    }
}