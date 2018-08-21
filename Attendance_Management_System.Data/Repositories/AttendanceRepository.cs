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

        public List<BCClass> GetActiveClasses()
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCClasses.Where(c => c.IsActive).ToList();
            }
        }

        public List<BCTeacherSubject> GetActiveTeachersSubjects()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeacherSubjects
                    .Include(s => s.Teacher)
                    .Where(t => t.Teacher.IsActive)
                    .ToList();
            }
        }

        public List<BCAttendance> GetAttendanceByDate(int classId, DateTime date)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCAttendances
                    .Include(a => a.StudentClass.Student)
                    .Where(a => a.StudentClass.BCClassId == classId && a.Date == date)
                    .ToList();
            }
        }

        public List<BCStudentClass> GetClassList(int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCStudentClasses
                    .Include(s => s.Student)
                    .Where(s => s.BCClassId == classId && s.IsActive && s.Student.IsActive)
                    .ToList();
            }
        }

        public void AddAttendance(List<BCAttendance> attendance)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.BCAttendances.AddRange(attendance);
                dbContext.SaveChanges();
            }
        }

        public void DeleteDaysAttendance(DateTime date, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Database.ExecuteSqlCommand("DELETE a FROM BCAttendances a JOIN BCStudentClasses s ON s.BCStudentClassId = a.BCStudentClassId" +
                    " WHERE a.Date = {0} AND s.BCClassId = {1}", date, classId);
            }
        }

        public void UpdateAttendance(BCAttendance attendance)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<BCAttendance>(attendance).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public DateTime? GetPreviousDate(DateTime date, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var attendance = dbContext.BCAttendances
                    .Include(a => a.StudentClass)
                    .OrderByDescending(a => a.Date)
                    .FirstOrDefault(a => a.Date < date && a.StudentClass.BCClassId == classId);

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
                var attendance = dbContext.BCAttendances
                    .Include(a => a.StudentClass)
                    .OrderBy(a => a.Date)
                    .FirstOrDefault(a => a.Date > date && a.StudentClass.BCClassId == classId);

                if(attendance != null)
                {
                    return attendance.Date;
                }
                return null;
            }
        }

        public List<BCAttendance> GetDaysAbsentees(DateTime date, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCAttendances
                    .Where(a => a.Date == date && a.Status != Status.Present && a.StudentClass.BCClassId == classId)
                    .Include(a => a.StudentClass.Student)
                    .Include(a => a.StudentClass.Class)
                    .Include(a => a.TeacherSubject.Teacher)
                    .ToList();                     
            }
        }
    }
}