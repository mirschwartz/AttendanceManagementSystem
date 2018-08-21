using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Repositories
{
    public class AdminRepository
    {
        private string _connectionString { get; set; }

        public AdminRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        //Students:
        public List<BCStudent> GetAllStudents()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var settings = dbContext.Settings.FirstOrDefault();
                return dbContext.BCStudents
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
                return dbContext.BCStudents.FirstOrDefault(s => s.BCStudentId == studentId);
            }
        }

        public int AddStudent(BCStudent student)
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.BCStudents.Add(student);
                dbContext.SaveChanges();

                return student.BCStudentId;
            }
        }

        public void UpdateStudent(BCStudent student)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<BCStudent>(student).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public void AddStudentClass(BCStudentClass studentClass)
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.BCStudentClasses.Add(studentClass);
                dbContext.SaveChanges();
            }
        }

        public void MarkStudentClassesInactive(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var studentclasses = dbContext.BCStudentClasses.Where(s => s.BCStudentId == studentId).ToList();

                foreach(var c in studentclasses)
                {
                    c.IsActive = false;
                    dbContext.Entry<BCStudentClass>(c).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
        }

        public void UpdateStudentClass(int studentId, BCStudentClass studentClass)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                //var alreadyExists = dbContext.StudentClasses.FirstOrDefault(s => s.StudentId == studentId && s.ClassId == classId) != null ? true : false;

                if (studentClass.BCStudentClassId != 0)
                {
                    //StudentClass studentClass = dbContext.StudentClasses.FirstOrDefault(s => s.StudentId == studentId && s.ClassId == classId);
                    //studentClass.IsActive = true;

                    dbContext.Entry<BCStudentClass>(studentClass).State = EntityState.Modified;
                }
                else
                {
                    BCStudentClass NewStudentClass = new BCStudentClass { BCStudentId = studentId, BCClassId = studentClass.BCClassId, IsActive = true };

                    dbContext.BCStudentClasses.Add(NewStudentClass);
                }

                dbContext.SaveChanges();
            }
        }

        public List<BCStudentClass> GetStudentClasses(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCStudentClasses
                    .Where(s => s.BCStudentId == studentId)
                    .ToList();
            }
        }

        //Classes
        public List<BCClass> GetAllClasses()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCClasses.ToList();
            }
        }

        public List<BCClass> GetActiveClasses()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCClasses.Where(c => c.IsActive).ToList();
            }
        }

        public BCClass GetClass(int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCClasses.FirstOrDefault(c => c.BCClassId == classId);
            }
        }

        public void AddClass(BCClass c)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.BCClasses.Add(c);
                dbContext.SaveChanges();
            }
        }

        public void UpdateClass(BCClass c)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<BCClass>(c).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        //Teachers
        public List<BCTeacher> GetAllTeachers()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeachers.Include(t => t.TeacherSubjects).ToList();
            }
        }

        public BCTeacher GetTeacherWithSubjects(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeachers
                    .Include(t => t.TeacherSubjects)
                    .FirstOrDefault(t => t.BCTeacherId == teacherId);
            }
        }

        public BCTeacher GetTeacher(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeachers.FirstOrDefault(t => t.BCTeacherId == teacherId);
            }
        }

        public List<BCTeacherSubject> GetTeacherSubjects(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeacherSubjects.Where(t => t.BCTeacherId == teacherId).ToList();
            }
        }

        public List<BCTeacherSubject> GetActiveTeacherSubjects()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCTeacherSubjects.Where(t => t.IsActive).Include(t => t.Teacher).ToList();
            }
        }

        public void AddTeacher(BCTeacher teacher)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.BCTeachers.Add(teacher);
                dbContext.SaveChanges();
            }
        }

        public void AddTeacherSubject (BCTeacherSubject subject)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.BCTeacherSubjects.Add(subject);
                dbContext.SaveChanges();
            }
        }

        public void UpdateTeacher(BCTeacher teacher)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<BCTeacher>(teacher).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public void MarkTeacherSubjectsInactive(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var teacherSubjects = dbContext.BCTeacherSubjects.Where(t => t.BCTeacherId == teacherId).ToList();

                foreach (var s in teacherSubjects)
                {
                    s.IsActive = false;
                    dbContext.Entry<BCTeacherSubject>(s).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
        }

        public void UpdateTeacherSubjects(int teacherId, BCTeacherSubject subject)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                if (subject.BCTeacherSubjectId != 0)
                {
                    dbContext.Entry<BCTeacherSubject>(subject).State = EntityState.Modified;
                }
                else
                {
                    BCTeacherSubject teacherSubject = new BCTeacherSubject { BCTeacherId = teacherId, Subject = subject.Subject, IsActive = true };

                    dbContext.BCTeacherSubjects.Add(teacherSubject);
                }

                dbContext.SaveChanges();
            }
        }

        //SCHEDULE

        public List<BCSchedule> GetSchedule(int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCSchedule.Where(s => s.BCClassId == classId).ToList();
            }
        }

        public List<BCSchedule> GetDailySchedule(int classId, DayOfWeek day)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.BCSchedule.Where(s => s.BCClassId == classId && s.DayOfWeek == day).ToList();
            }
        }

        public void AddSchedule(List<BCSchedule> schedule)
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.BCSchedule.AddRange(schedule);

                dbContext.SaveChanges();
            }
        }

        public void DeleteSchedule(int classId, DayOfWeek day)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Database.ExecuteSqlCommand("DELETE FROM BCSchedules WHERE DayOfWeek = {0} and BCClassId = {1}",
                    day, classId);
            }
        }
    }
}