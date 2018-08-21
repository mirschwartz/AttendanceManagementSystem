using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Repositories
{
    public class TeacherRepository
    {
        private readonly string _connectionString;

        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BCTeacher> GetActiveTeachers()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var settings = dbContext.Settings.FirstOrDefault();

                return dbContext.BCTeachers
                    .Include(t => t.TeacherSubjects.Select(s => s.Attendances.Select(a => a.StudentClass)))
                    .Include(t => t.TeacherSubjects.Select(s => s.Attendances.Select(a => a.StudentClass.Class)))
                    .Include(t => t.TeacherSubjects.Select(s => s.Attendances.Select(a => a.StudentClass.Student)))
                    .Where(t => t.IsActive)
                    .ToList();
            }
        }

        public BCTeacher GetTeacher(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeachers.Include(t => t.TeacherSubjects).FirstOrDefault(t => t.BCTeacherId == teacherId);
            }
        }

        public List<BCAttendance> GetTeachersAttendance(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var settings = dbContext.Settings.FirstOrDefault();

                return dbContext.BCAttendances
                    .Where(a => a.TeacherSubject.BCTeacherId == teacherId && (a.Status == Status.ExcusedAbsence || a.Status == Status.UnexcusedAbsence))
                    .Include(a => a.StudentClass.Student)
                    .Include(a => a.StudentClass.Class)
                    .Include(a => a.TeacherSubject)
                    .Where(s => (s.StudentClass.Student.YearStart == settings.YearStart && s.StudentClass.Student.YearEnd == settings.YearEnd) || (s.StudentClass.Student.YearStart == 0 && s.StudentClass.Student.YearEnd == 0))
                    .ToList();
            }
        }

        public List<BCStudent> GetActiveStudents(int TeacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var settings = dbContext.Settings.FirstOrDefault();

                return dbContext.BCStudents
                    .Where(s => s.IsActive)
                    .Include(s => s.StudentClasses.Select(c => c.Attendances.Select(t => t.TeacherSubject)))
                    .Include(s => s.StudentClasses.Select(c => c.Class))
                    .Where(s => (s.YearStart == settings.YearStart && s.YearEnd == settings.YearEnd) || (s.YearStart == 0 && s.YearEnd == 0))
                    .ToList();
            }
        }

        public Settings GetSettings()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Settings.FirstOrDefault();
            }
        }
    }
}