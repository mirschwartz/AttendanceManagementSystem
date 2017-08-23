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

        public List<Student> GetActiveStudents()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Students
                    .Where(s => s.IsActive)
                    .Include(s => s.StudentClasses.Select(c => c.Attendances))
                    .Include(s => s.StudentClasses.Select(c => c.Class))
                    .ToList();
            }
        }

        public Student GetStudent(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Students.Include(s => s.StudentClasses.Select(c => c.Class)).FirstOrDefault(s => s.StudentId == studentId);
            }
        }

        public List<Attendance> GetStudentAttendance(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Attendances
                    .Include(a => a.TeacherSubject.Teacher)
                    .Include(a => a.StudentClass.Class)
                    .Where(a => a.StudentClass.StudentId == studentId && (a.Status == Status.ExcusedAbsence || a.Status == Status.UnexcusedAbsence))
                    .ToList();
            }
        }
    }
}
