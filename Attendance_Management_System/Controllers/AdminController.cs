using Attendance_Management_System.Data.Models;
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
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly AdminRepository _adminRepo;

        public AdminController()
        {
            _adminService = new AdminService(ConnectionString.DB);
            _adminRepo = new AdminRepository(ConnectionString.DB);
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Students()
        {
            AdminStudentsVM vm = new AdminStudentsVM()
            {
                Classes = _adminRepo.GetAllClasses(),
                Students = _adminRepo.GetAllStudents()                
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult NewStudent(Student student, List<int> classId)
        {
            _adminService.AddStudent(student, classId);

            return Redirect("/admin/students");
        }

        [HttpPost]
        public ActionResult UpdateStudent(Student student, List<int> classId)
        {
            _adminService.EditStudent(student, classId);

            return Redirect("/admin/students");
        }

        [HttpPost]
        public ActionResult ChangeStudentActiveState(int studentId, bool isActive)
        {
            _adminService.ChangeStudentActiveState(studentId, s =>
            {
                s.IsActive = isActive;
            });

            return Redirect("/admin/students");
        }

        public ActionResult GetStudentClasses(int studentId)
        {
            return Json(new { Student = _adminRepo.GetStudent(studentId), StudentClasses = _adminRepo.GetStudentClasses(studentId) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Teachers()
        {
            AdminTeachersVM vm = new AdminTeachersVM
            {
                Teachers = _adminRepo.GetAllTeachers()
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult NewTeacher(Teacher teacher)
        {
            _adminService.AddTeacher(teacher);

            return Redirect("/admin/teachers");
        }

        [HttpPost]
        public ActionResult EditTeacher(Teacher teacher, List<TeacherSubject> subjects)
        {
            _adminService.UpdateTeacher(teacher, subjects);

            return Redirect("/admin/teachers");
        }

        [HttpPost]
        public ActionResult ChangeTeacherActiveState(int teacherId, bool isActive)
        {
            _adminService.ChangeTeacherActiveState(teacherId, s =>
            {
                s.IsActive = isActive;
            });

            return Redirect("/admin/teachers");
        }

        public ActionResult GetTeacher(int teacherId)
        {
            var teacher = _adminRepo.GetTeacher(teacherId);

            return Json(new { Teacher = new { teacher.TeacherId, teacher.Title, teacher.FirstName, teacher.LastName,
                teacher.CellPhone, teacher.HomePhone, IsActive = (teacher.IsActive ? 1 : 0)},
                Subjects = _adminRepo.GetTeacherSubjects(teacherId)}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Classes()
        {
            AdminClassesVM vm = new AdminClassesVM
            {
                Classes = _adminRepo.GetAllClasses()
            };

            return View(vm);
        }

        public ActionResult GetClasses()
        {
            return Json(_adminRepo.GetActiveClasses(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewClass(Class c)
        {
            _adminRepo.AddClass(c);

            return Redirect("/admin/classes");
        }

        [HttpPost]
        public ActionResult UpdateClass(Class c)
        {
            _adminRepo.UpdateClass(c);

            return Redirect("/admin/classes");
        }

        [HttpPost]
        public ActionResult ChangeClassActiveState(int classId, bool isActive)
        {
            _adminService.ChangeClassActiveState(classId, c =>
            {
                c.IsActive = isActive;
            });

            return Redirect("/admin/classes");
        }
    }
}