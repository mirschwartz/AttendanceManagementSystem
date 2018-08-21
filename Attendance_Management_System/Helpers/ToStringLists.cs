using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.Helpers
{
    public class ToStringLists
    {
        public static string StringClasses(ICollection<BCStudentClass> studentClasses)
        {
            string classes = "";

            for (var c = 0; c < studentClasses.Count; c++)
            {
                var thisClass = studentClasses.ToList()[c];
                classes += thisClass.Class.ClassName;
                if (c < studentClasses.Count - 1)
                {
                    classes += ", ";
                }
            }

            return classes;
        }

        public static string StringSubjects(ICollection<BCTeacherSubject> teacherSubjects)
        {
            var subjects = "";
            for (int s = 0; s < teacherSubjects.Count; s++)
            {
                subjects += teacherSubjects.ToList()[s].Subject;

                if (s < teacherSubjects.Count - 1)
                {
                    subjects += ", ";
                }
            }
            return subjects;
        }
    }
}