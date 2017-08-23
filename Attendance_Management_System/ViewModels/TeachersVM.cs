using Attendance_Management_System.Data.Models;
using Attendance_Management_System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.ViewModels
{
    public class TeachersVM
    {
        public List<TeacherVM> Teachers { get; set; }

        public TeachersVM(List<Teacher> teachers)
        {
            this.Teachers = new List<TeacherVM>();

            foreach(Teacher t in teachers)
            {
                var absentees = 0;
                var totalPresent = 0;
                var totalClasses = 0;

                for(int s = 0; s < t.TeacherSubjects.Count; s++)
                {
                    var thisSubject = t.TeacherSubjects.ToList()[s];

                    foreach(var a in thisSubject.Attendances)
                    {
                        if(a.Status == Status.ExcusedAbsence || a.Status == Status.UnexcusedAbsence)
                        {
                            absentees++;
                        }
                        else
                        {
                            totalPresent++;
                        }
                        totalClasses++;
                    }
                }

                this.Teachers.Add(new TeacherVM
                {
                    TeacherId = t.TeacherId,
                    Teacher = $"{t.Title} {t.FirstName} {t.LastName}",
                    Subjects = ToStringLists.StringSubjects(t.TeacherSubjects),
                    Absentees = absentees,
                    Percentage = totalPresent == 0 && totalClasses != 0 ? 0 : ((totalPresent == 0 ? 1 : totalPresent) * 100) / (totalClasses == 0 ? 1 : totalClasses)
                });
            }
        }
    }
}