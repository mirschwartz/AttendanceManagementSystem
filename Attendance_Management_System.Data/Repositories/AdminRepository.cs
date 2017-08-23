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
        public List<Student> GetAllStudents()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Students
                    .Include(s => s.StudentClasses.Select(c => c.Attendances))
                    .Include(s => s.StudentClasses.Select(c => c.Class))
                    .ToList();
            }
        }

        public Student GetStudent(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Students.FirstOrDefault(s => s.StudentId == studentId);
            }
        }

        public int AddStudent(Student student)
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Students.Add(student);
                dbContext.SaveChanges();

                return student.StudentId;
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<Student>(student).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public void AddStudentClass(StudentClass studentClass)
        {
            using(var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.StudentClasses.Add(studentClass);
                dbContext.SaveChanges();
            }
        }

        public void MarkStudentClassesInactive(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var studentclasses = dbContext.StudentClasses.Where(s => s.StudentId == studentId).ToList();

                foreach(var c in studentclasses)
                {
                    c.IsActive = false;
                    dbContext.Entry<StudentClass>(c).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
        }

        public void UpdateStudentClass(int studentId, int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var alreadyExists = dbContext.StudentClasses.FirstOrDefault(s => s.StudentId == studentId && s.ClassId == classId) != null ? true : false;

                if (alreadyExists)
                {
                    StudentClass studentClass = dbContext.StudentClasses.FirstOrDefault(s => s.StudentId == studentId && s.ClassId == classId);
                    studentClass.IsActive = true;

                    dbContext.Entry<StudentClass>(studentClass).State = EntityState.Modified;
                }
                else
                {
                    StudentClass studentClass = new StudentClass { StudentId = studentId, ClassId = classId, IsActive = true };

                    dbContext.StudentClasses.Add(studentClass);
                }

                dbContext.SaveChanges();
            }
        }

        public List<int> GetStudentClasses(int studentId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.StudentClasses
                    .Where(s => s.StudentId == studentId && s.IsActive)
                    .Select(s => s.ClassId)
                    .ToList();
            }
        }

        //Classes
        public List<Class> GetAllClasses()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Classes.ToList();
            }
        }

        public List<Class> GetActiveClasses()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Classes.Where(c => c.IsActive).ToList();
            }
        }

        public Class GetClass(int classId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Classes.FirstOrDefault(c => c.ClassId == classId);
            }
        }

        public void AddClass(Class c)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Classes.Add(c);
                dbContext.SaveChanges();
            }
        }

        public void UpdateClass(Class c)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<Class>(c).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        //Teachers
        public List<Teacher> GetAllTeachers()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Teachers.Include(t => t.TeacherSubjects).ToList();
            }
        }

        public Teacher GetTeacherWithSubjects(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Teachers
                    .Include(t => t.TeacherSubjects)
                    .FirstOrDefault(t => t.TeacherId == teacherId);
            }
        }

        public Teacher GetTeacher(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Teachers.FirstOrDefault(t => t.TeacherId == teacherId);
            }
        }

        public List<TeacherSubject> GetTeacherSubjects(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.TeacherSubjects.Where(t => t.TeacherId == teacherId).ToList();
            }
        }

        public void AddTeacher(Teacher teacher)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Teachers.Add(teacher);
                dbContext.SaveChanges();
            }
        }

        public void AddTeacherSubject (TeacherSubject subject)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.TeacherSubjects.Add(subject);
                dbContext.SaveChanges();
            }
        }

        public void UpdateTeacher(Teacher teacher)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Entry<Teacher>(teacher).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public void MarkTeacherSubjectsInactive(int teacherId)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                var teacherSubjects = dbContext.TeacherSubjects.Where(t => t.TeacherId == teacherId).ToList();

                foreach (var s in teacherSubjects)
                {
                    s.IsActive = false;
                    dbContext.Entry<TeacherSubject>(s).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
        }

        public void UpdateTeacherSubjects(int teacherId, TeacherSubject subject)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                //var alreadyExists = dbContext.TeacherSubjects.FirstOrDefault(s => s.TeacherId == teacherId && s.Subject == subject) != null ? true : false;

                if (subject.TeacherSubjectId != 0)
                {
                    //TeacherSubject teacherSubject = dbContext.TeacherSubjects.FirstOrDefault(s => s.TeacherId == teacherId && s.Subject == subject);
                    //teacherSubject.IsActive = true;

                    dbContext.Entry<TeacherSubject>(subject).State = EntityState.Modified;
                }
                else
                {
                    TeacherSubject teacherSubject = new TeacherSubject { TeacherId = teacherId, Subject = subject.Subject, IsActive = true };

                    dbContext.TeacherSubjects.Add(teacherSubject);
                }

                dbContext.SaveChanges();
            }
        }
    }
}