using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Repositories
{
    public class AttendanceRepository
    {
        private readonly string _connectionString;

        public AttendanceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Class> GetActiveClasses()
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Classes.Where(c => c.IsActive).ToList();
            }
        }

        public List<TeacherSubject> GetActiveTeachersSubjects()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.TeacherSubjects
                    .Include(s => s.Teacher)
                    .Where(t => t.Teacher.IsActive)
                    .ToList();
            }
        }

        public List<Attendance> GetAttendanceByDate(int classId, DateTime date)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Attendances
                    .Include(a => a.StudentClass.Student)
                    .Where(a => a.StudentClass.ClassId == classId && a.Date == date)
                    .ToList();
            }
        }

        public List<StudentClass> GetClassList(int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.StudentClasses
                    .Include(s => s.Student)
                    .Where(s => s.ClassId == classId && s.IsActive && s.Student.IsActive)
                    .ToList();
            }
        }

        public void AddAttendance(List<Attendance> attendance)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Attendances.AddRange(attendance);
                dbContext.SaveChanges();
            }
        }

        public void DeleteDaysAttendance(DateTime date, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Database.ExecuteSqlCommand("DELETE a FROM Attendances a JOIN StudentClasses s ON s.StudentClassId = a.StudentClassId" +
                    " WHERE a.Date = {0} AND s.ClassId = {1}", date, classId);
            }
        }

        public void UpdateAttendance(Attendance attendance)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<Attendance>(attendance).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public DateTime? GetPreviousDate(DateTime date, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var attendance = dbContext.Attendances
                    .Include(a => a.StudentClass)
                    .OrderByDescending(a => a.Date)
                    .FirstOrDefault(a => a.Date < date && a.StudentClass.ClassId == classId);

                if (attendance != null)
                {
                    return attendance.Date;
                }
                return null;
            }
        }

        public DateTime? GetNextDate(DateTime date, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var attendance = dbContext.Attendances
                    .Include(a => a.StudentClass)
                    .OrderBy(a => a.Date)
                    .FirstOrDefault(a => a.Date > date && a.StudentClass.ClassId == classId);

                if(attendance != null)
                {
                    return attendance.Date;
                }
                return null;
            }
        }

        public List<Attendance> GetDaysAbsentees(DateTime date, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Attendances
                    .Where(a => a.Date == date && a.Status != Status.Present && a.StudentClass.ClassId == classId)
                    .Include(a => a.StudentClass.Student)
                    .Include(a => a.StudentClass.Class)
                    .Include(a => a.TeacherSubject.Teacher)
                    .ToList();                     
            }
        }
    }
}
