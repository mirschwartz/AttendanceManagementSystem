using Attendance_Management_System.Data.Repositories;
using Attendance_Management_System.Helpers;
using Attendance_Management_System.Services;
using Attendance_Management_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance_Management_System.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly StudentRepository _studentRepo;

        public StudentController()
        {
            _studentService = new StudentService(ConnectionString.DB);
            _studentRepo = new StudentRepository(ConnectionString.DB);
        }

        // GET: Student
        public ActionResult Index()
        {
            StudentsVM vm = new StudentsVM(_studentRepo.GetActiveStudents());
            return View(vm);
        }

        public ActionResult StudentRecord(int studentId)
        {
            var student = _studentRepo.GetStudent(studentId);

            StudentRecordVM vm = new StudentRecordVM
            {
                Student = student,
                Classes = ToStringLists.StringClasses(student.StudentClasses),
                Attendance = _studentRepo.GetStudentAttendance(studentId)
            };

            return View(vm);
        }
    }
}