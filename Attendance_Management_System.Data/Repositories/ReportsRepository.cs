using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Repositories
{
    public class ReportsRepository
    {
        private readonly string _connectionString;

        public ReportsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BCAttendance> GetReport(int? classId, int? student, int? teacher, DateTime? date, Status? status)
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var attendance = dbContext.BCAttendances
                    .Include(a => a.StudentClass.Student)
                    .Include(a => a.TeacherSubject.Teacher)
                    .Include(a => a.StudentClass.Class)
                    .ToList();

                attendance = classId != 0 ? attendance.Where(a => a.StudentClass.BCClassId == classId).ToList() : attendance;
                attendance = student != 0 ? attendance.Where(a => a.StudentClass.BCStudentId == student).ToList() : attendance;
                attendance = teacher != 0 ? attendance.Where(a => a.TeacherSubject.BCTeacherId == teacher).ToList() : attendance;
                attendance = date != null ? attendance.Where(a => a.Date == date).ToList() : attendance;
                attendance = status != 0 ? attendance.Where(a => a.Status == status).ToList() : attendance;

                return attendance;
            }

        }

        public List<BCClass> GetAllClasses()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCClasses.ToList();
            }
        }

        public List<BCTeacher> GetAllTeachers()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeachers.ToList();
            }
        }

        public List<BCStudent> GetAllStudents()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var settings = dbContext.Settings.FirstOrDefault();
                return dbContext.BCStudents.Where(s => s.YearStart == settings.YearStart && s.YearEnd == settings.YearEnd).ToList();
            }
        }
    }
}
