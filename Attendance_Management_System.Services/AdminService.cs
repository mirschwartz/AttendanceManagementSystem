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

        public void AddStudent(BCStudent student, List<BCStudentClass> classIds)
        {
            classIds = classIds.Where(c => c.BCClassId != 0).ToList();

            var studentId = _adminRepo.AddStudent(student);

            foreach(var c in classIds)
            {
                c.BCStudentId = studentId;
                c.IsActive = true;
                _adminRepo.AddStudentClass(c);
            }
        }

        public void EditStudent(BCStudent student, List<BCStudentClass> classId)
        {
            classId = classId.Where(c => c.BCClassId != 0).ToList();
            _adminRepo.UpdateStudent(student);

            _adminRepo.MarkStudentClassesInactive(student.BCStudentId);

            foreach (var c in classId)
            {
                _adminRepo.UpdateStudentClass(student.BCStudentId, c);
            }
        }

        public void ChangeStudentActiveState(int studentId, Action<BCStudent> action)
        {
            var thisStudent = _adminRepo.GetStudent(studentId);
            if(thisStudent != null)
            {
                action?.Invoke(thisStudent);
                _adminRepo.UpdateStudent(thisStudent);
            }
        }

        public void ChangeClassActiveState(int classId, Action<BCClass> action)
        {
            var thisClass = _adminRepo.GetClass(classId);
            if (thisClass != null)
            {
                action?.Invoke(thisClass);
                _adminRepo.UpdateClass(thisClass);
            }
        }

        public void ChangeTeacherActiveState(int teacherId, Action<BCTeacher> action)
        {
            var thisTeacher = _adminRepo.GetTeacherWithSubjects(teacherId);
            if (thisTeacher != null)
            {
                action?.Invoke(thisTeacher);
                _adminRepo.UpdateTeacher(thisTeacher);
            }
        }

        public void AddTeacher(BCTeacher teacher)
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

        public void UpdateTeacher(BCTeacher teacher, List<BCTeacherSubject> subjects)
        {
            //var teacherId = teacher.TeacherId;
            subjects = subjects.Where(s => s.Subject != null && s.Subject != "").ToList();

            _adminRepo.UpdateTeacher(teacher);

            _adminRepo.MarkTeacherSubjectsInactive(teacher.BCTeacherId);

            foreach (var subject in subjects)
            {
                _adminRepo.UpdateTeacherSubjects(teacher.BCTeacherId, subject);
            }
        }

        public void AddSchedule(List<BCSchedule> schedule)
        {
            schedule = schedule.Where(s => s.BCTeacherSubjectId != 0).ToList();

            _adminRepo.AddSchedule(schedule);
        }


        public void UpdateSchedule(List<BCSchedule> schedule, DayOfWeek day, int classId)
        {
            _adminRepo.DeleteSchedule(classId, day);
            AddSchedule(schedule);
        }
    }
}