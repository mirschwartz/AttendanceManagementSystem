using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Repositories
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BCStudent> GetActiveStudents()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var settings = dbContext.Settings.FirstOrDefault();
                return dbContext.BCStudents
                    .Where(s => s.IsActive)
                    .Include(s => s.StudentClasses.Select(c => c.Attendances))
                    .Include(s => s.StudentClasses.Select(c => c.Class))
                    .Where(s => (s.YearStart == settings.YearStart && s.YearEnd == settings.YearEnd) || (s.YearStart == 0 && s.YearEnd == 0))
                    .ToList();
            }
        }

        public BCStudent GetStudent(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var settings = dbContext.Settings.FirstOrDefault();

                return dbContext.BCStudents.Include(s => s.StudentClasses.Select(c => c.Class))
                    .Where(s => (s.YearStart == settings.YearStart && s.YearEnd == settings.YearEnd) || (s.YearStart == 0 && s.YearEnd == 0))
                    .FirstOrDefault(s => s.BCStudentId == studentId);
            }
        }

        public List<BCAttendance> GetStudentAttendance(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCAttendances
                    .Include(a => a.TeacherSubject.Teacher)
                    .Include(a => a.StudentClass.Class)
                    .Where(a => a.StudentClass.BCStudentId == studentId && (a.Status != Status.Present))
                    .ToList();
            }
        }

        public void EditRecord(BCAttendance record)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Database.ExecuteSqlCommand(@"UPDATE BCAttendances SET Status = {0} WHERE BCAttendanceId = {1}", record.Status, record.BCAttendanceId);
            }
        }
    }
}
