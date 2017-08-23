using Attendance_Management_System.Data.Models;
using Attendance_Management_System.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepo;

        public AdminService()
        {

        }

        public AdminService(string connectionString)
        {
            _adminRepo = new AdminRepository(connectionString);
        }

        public void AddStudent(Student student, List<int> classIds)
        {
            classIds = classIds.Where(c => c != 0).ToList();

            var studentId = _adminRepo.AddStudent(student);

            foreach(var c in classIds)
            {
                _adminRepo.AddStudentClass(new StudentClass { StudentId = studentId, ClassId = c, IsActive = true });
            }
        }

        public void EditStudent(Student student, List<int> classId)
        {
            classId = classId.Where(c => c != 0).ToList();
            _adminRepo.UpdateStudent(student);

            _adminRepo.MarkStudentClassesInactive(student.StudentId);

            foreach (int c in classId)
            {
                _adminRepo.UpdateStudentClass(student.StudentId, c);
            }
        }

        public void ChangeStudentActiveState(int studentId, Action<Student> action)
        {
            var thisStudent = _adminRepo.GetStudent(studentId);
            if(thisStudent != null)
            {
                action?.Invoke(thisStudent);
                _adminRepo.UpdateStudent(thisStudent);
            }
        }

        public void ChangeClassActiveState(int classId, Action<Class> action)
        {
            var thisClass = _adminRepo.GetClass(classId);
            if (thisClass != null)
            {
                action?.Invoke(thisClass);
                _adminRepo.UpdateClass(thisClass);
            }
        }

        public void ChangeTeacherActiveState(int teacherId, Action<Teacher> action)
        {
            var thisTeacher = _adminRepo.GetTeacherWithSubjects(teacherId);
            if (thisTeacher != null)
            {
                action?.Invoke(thisTeacher);
                _adminRepo.UpdateTeacher(thisTeacher);
            }
        }

        public void AddTeacher(Teacher teacher)
        {
            if(teacher.TeacherSubjects != null)
            {
                teacher.TeacherSubjects = teacher.TeacherSubjects.Where(s => s.Subject != "" && s.Subject != null).ToList();
                foreach(var subject in teacher.TeacherSubjects)
                {
                    subject.IsActive = true;
                }
            }
            _adminRepo.AddTeacher(teacher);
        }

        public void UpdateTeacher(Teacher teacher, List<TeacherSubject> subjects)
        {
            //var teacherId = teacher.TeacherId;
            subjects = subjects.Where(s => s.Subject != null && s.Subject != "").ToList();

            _adminRepo.UpdateTeacher(teacher);

            _adminRepo.MarkTeacherSubjectsInactive(teacher.TeacherId);

            foreach (var subject in subjects)
            {
                _adminRepo.UpdateTeacherSubjects(teacher.TeacherId, subject);
            }
        }
    }
}