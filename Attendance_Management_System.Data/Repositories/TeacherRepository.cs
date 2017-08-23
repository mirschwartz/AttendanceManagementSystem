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

        public List<Teacher> GetActiveTeachers()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Teachers
                    .Include(t => t.TeacherSubjects.Select(s => s.Attendances.Select(a => a.StudentClass)))
                    .Where(t => t.IsActive)
                    .ToList();
            }
        }

        public Teacher GetTeacher(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Teachers.Include(t => t.TeacherSubjects).FirstOrDefault(t => t.TeacherId == teacherId);
            }
        }

        public List<Attendance> GetTeachersAttendance(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Attendances
                    .Where(a => a.TeacherSubject.TeacherId == teacherId && (a.Status == Status.ExcusedAbsence || a.Status == Status.UnexcusedAbsence))
                    .Include(a => a.StudentClass.Student)
                    .Include(a => a.StudentClass.Class)
                    .Include(a => a.TeacherSubject)
                    .ToList();
            }
        }
    }
}