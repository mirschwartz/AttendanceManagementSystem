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
    public class TeacherController : Controller
    {
        private readonly TeacherService _teacherService;
        private readonly TeacherRepository _teacherRepo;

        public TeacherController()
        {
            _teacherService = new TeacherService(ConnectionString.DB);
            _teacherRepo = new TeacherRepository(ConnectionString.DB);
        }

        // GET: Teacher
        public ActionResult Index()
        {
            TeachersVM vm = new TeachersVM(_teacherRepo.GetActiveTeachers(), _teacherRepo.GetSettings());

            return View(vm);
        }

        public ActionResult TeacherRecord(int teacherId)
        {
            var teacher = _teacherRepo.GetTeacher(teacherId);

            TeacherRecordVM vm = new TeacherRecordVM
            {
                Teacher = teacher,
                Subjects = ToStringLists.StringSubjects(teacher.TeacherSubjects),
                Attendance = _teacherRepo.GetTeachersAttendance(teacherId)
            };
            return View(vm);
        }

        public ActionResult NewTeacherRecord(int teacherId)
        {
            var teacher = _teacherRepo.GetTeacher(teacherId);

            TeacherRecordsVM vm = new TeacherRecordsVM(_teacherRepo.GetActiveStudents(teacherId), teacherId);

            vm.Teacher = teacher;

            vm.Subjects = ToStringLists.StringSubjects(teacher.TeacherSubjects);

            return View(vm);
        }
    }
}