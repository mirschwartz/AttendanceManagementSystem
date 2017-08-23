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
    public class AttendanceController : Controller
    {
        private readonly AttendanceService _attendanceService;
        private readonly AttendanceRepository _attendanceRepo;

        public AttendanceController()
        {
            _attendanceService = new AttendanceService(ConnectionString.DB);
            _attendanceRepo = new AttendanceRepository(ConnectionString.DB);
        }

        // GET: Attendance
        public ActionResult Index()
        {
            AttendanceVM vm = new AttendanceVM
            {
                Classes = _attendanceRepo.GetActiveClasses(),
                Subjects = _attendanceRepo.GetActiveTeachersSubjects()
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult MarkAttendance(List<List<Attendance>> Class, List<TeacherSubject> teacher, DateTime date, int classId)
        {
            _attendanceService.MarkAttendance(Class, teacher, date, classId);

            return Redirect($"/attendance/excuseabsentees?date={date}&classId={classId}");
        }

        [HttpPost]
        public ActionResult UpdateAttendance(List<List<Attendance>> Class, List<TeacherSubject> teacher, DateTime date, int classId)
        {
            _attendanceService.UpdateAttendance(date, classId, Class, teacher);
            return Redirect($"/attendance/excuseabsentees?date={date}&classId={classId}");
        }

        public ActionResult ExcuseAbsentees(DateTime date, int classId)
        {
            ExcuseAbsenteesVM vm = new ExcuseAbsenteesVM
            {
                AbsentStudents = _attendanceRepo.GetDaysAbsentees(date, classId)
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult ExcuseAbsentees(List<Attendance> attendance)
        {
            _attendanceService.ExcuseAbsentees(attendance);

            return Redirect("/attendance");
        }

        public ActionResult GetTeachers()
        {
            return Json(_attendanceRepo.GetActiveTeachersSubjects().Select(t => new { t.TeacherSubjectId, t.Teacher.Title, t.Teacher.LastName, t.Subject }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassList(int classId)
        {
            var students = _attendanceRepo.GetClassList(classId);
            return Json(students.Select(s => new { s.StudentClassId, s.Student.FirstName, s.Student.LastName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassListByDate(DateTime currentDate, string btn, int classId)
        {
            DateTime? date = _attendanceService.GetClassListByDate(currentDate, btn, classId);

            if (date == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(date.Value.ToShortDateString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPastAttendance(DateTime date, int classId)
        {
            var attendance = _attendanceRepo.GetAttendanceByDate(classId, date);

            return Json(attendance.Select(a => new
            {
                a.Date,
                a.Status,
                a.Period,
                a.notes,
                TeacherSubjectId = a.TeacherSubjectId,
                StudentId = a.StudentClass.StudentClassId,
                a.StudentClass.Student.FirstName,
                a.StudentClass.Student.LastName,
                a.StudentClass.ClassId
            }), JsonRequestBehavior.AllowGet);
        }
    }
}