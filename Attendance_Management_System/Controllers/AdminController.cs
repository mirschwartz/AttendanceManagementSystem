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

        [HttpGet]
        public ActionResult Students()
        {
            AdminStudentsVM vm = new AdminStudentsVM()
            {
                Classes = _adminRepo.GetAllClasses(),
                Students = _adminRepo.GetAllStudents().OrderBy(s => s.LastName).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult NewStudent(BCStudent student, List<BCStudentClass> classId)
        {
            _adminService.AddStudent(student, classId);

            return Redirect("/admin/students");
        }


        [HttpPost]
        public ActionResult UpdateStudent(BCStudent student, List<BCStudentClass> classId)
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
        public ActionResult NewTeacher(BCTeacher teacher)
        {
            _adminService.AddTeacher(teacher);

            return Redirect("/admin/teachers");
        }

        [HttpPost]
        public ActionResult EditTeacher(BCTeacher teacher, List<BCTeacherSubject> subjects)
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

            return Json(new { Teacher = new { teacher.BCTeacherId, teacher.Title, teacher.FirstName, teacher.LastName,
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
            return Json(_adminRepo.GetAllClasses(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewClass(BCClass c)
        {
            _adminRepo.AddClass(c);

            return Redirect("/admin/classes");
        }

        [HttpPost]
        public ActionResult UpdateClass(BCClass c)
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

        public ActionResult SchoolSchedules()
        {
            SchoolScheduleVM vm = new SchoolScheduleVM
            {
                Classes = _adminRepo.GetAllClasses()
            };

            return View(vm);
        }

        public ActionResult Schedule(int classId)
        {
            AdminScheduleVM vm = new AdminScheduleVM();

            vm.Class = _adminRepo.GetClass(classId);
            vm.TeacherSubjects = _adminRepo.GetActiveTeacherSubjects();
            vm.Schedule = _adminRepo.GetSchedule(classId);

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddSchedule(List<BCSchedule> schedule)
        {
            _adminService.AddSchedule(schedule);

            return Redirect("/Admin/Schedule?classId=" + schedule[0].BCClassId);
        }

        [HttpPost]
        public ActionResult UpdateSchedule(List<BCSchedule> schedule)
        {
            if(schedule.Count() > 0)
            {
                _adminService.UpdateSchedule(schedule, schedule[0].DayOfWeek, schedule[0].BCClassId);
            }
            return Redirect("/Admin/SchoolSchedules");
        }

        public ActionResult ScheduleDaily(DayOfWeek day, int classId)
        {
            var vm = new AdminScheduleDailyVM();

            vm.Class = _adminRepo.GetClass(classId);
            vm.TeacherSubjects = _adminRepo.GetActiveTeacherSubjects();
            vm.Schedule = _adminRepo.GetDailySchedule(classId, day);
            vm.DayOfWeek = day;

            return View(vm);
        }

        public ActionResult getSchedule(int classId)
        {
            var schedule = _adminRepo.GetSchedule(classId);

            return Json(schedule.Select(s => new { s.BCScheduleId, s.DayOfWeek, s.BCTeacherSubjectId, s.To, s.From }), JsonRequestBehavior.AllowGet);
        }

    }
}