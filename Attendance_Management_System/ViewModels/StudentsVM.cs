using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Attendance_Management_System.Data.Models;
using Attendance_Management_System.Helpers;

namespace Attendance_Management_System.ViewModels
{
    public class StudentsVM
    {
        public List<StudentVM> Students { get; set; }

        public StudentsVM(List<BCStudent> students)
        {
            this.Students = new List<StudentVM>();

            foreach(var s in students)
            {
                var totalAbsences = 0;
                var totalLatenesses = 0;
                var totalPresent = 0;
                var amountOfClasses = 0;

                for (var c = 0; c < s.StudentClasses.Count; c++)
                {
                    var thisClass = s.StudentClasses.ToList()[c];

                    foreach (var a in thisClass.Attendances)
                    {
                        if (a.StudentClass.Class.IsActive)
                        {
                            if (a.Status == Status.ExcusedAbsence || a.Status == Status.UnexcusedAbsence)
                            {
                                totalAbsences++;
                            }
                            else
                            {
                                if (a.Status == Status.UnexcusedLateness || a.Status == Status.ExcusedLateness)
                                {
                                    totalLatenesses++;
                                }
                                totalPresent++;
                            }
                            amountOfClasses++;
                        }
                    }
                }

                this.Students.Add(new StudentVM()
                {
                    StudentId = s.BCStudentId,
                    Name = $"{s.FirstName} {s.LastName}",
                    Class = ToStringLists.StringClasses(s.StudentClasses.Where(c => c.Class.IsActive).ToList()),
                    TotalAbsences = totalAbsences,
                    TotalLatenesses = totalLatenesses,
                    Percentage = totalPresent == 0 && amountOfClasses != 0 ? 0 : ((totalPresent == 0 ? 1 : totalPresent) * 100) / (amountOfClasses == 0 ? 1 : amountOfClasses)
                });
            }
        }
    }
}