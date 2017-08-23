using Attendance_Management_System.Data.Models;
using Attendance_Management_System.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Services
{
    public class AttendanceService
    {
        private readonly AttendanceRepository _attendanceRepo;

        public AttendanceService()
        {

        }

        public AttendanceService(string connectionString)
        {
            _attendanceRepo = new AttendanceRepository(connectionString);
        }

        public DateTime? GetClassListByDate(DateTime currentDate, string btn, int classId)
        {
            if (btn == "previous")
            {
                return _attendanceRepo.GetPreviousDate(currentDate, classId);
            }
            else if(btn == "next")
            {
                return _attendanceRepo.GetNextDate(currentDate, classId);
            }

            return null;
        }

        public void MarkAttendance(List<List<Attendance>> Class, List<TeacherSubject> teacher, DateTime date, int classId)
        {
            int index = 0;
            List<Attendance> masterList = new List<Attendance>();

            foreach (List<Attendance> attendance in Class)
            {
                int id = teacher[index].TeacherSubjectId;
                foreach (Attendance status in attendance)
                {
                    status.TeacherSubjectId = id;
                    status.Date = date;
                    masterList.Add(status);
                }
                index++;
            }

            masterList = masterList.Where(a => a.TeacherSubjectId != 0).ToList();

            _attendanceRepo.AddAttendance(masterList);
        }

        public void UpdateAttendance(DateTime date, int classId, List<List<Attendance>> attendance, List<TeacherSubject> teacher)
        {
            _attendanceRepo.DeleteDaysAttendance(date, classId);

            int index = 0;
            List<Attendance> masterList = new List<Attendance>();

            foreach (List<Attendance> a in attendance)
            {
                int id = teacher[index].TeacherSubjectId;

                foreach (Attendance status in a)
                {
                    status.TeacherSubjectId = id;
                    status.Date = date;
                    masterList.Add(status);
                }
                index++;
            }

            masterList = masterList.Where(a => a.TeacherSubjectId != 0).ToList();

            _attendanceRepo.AddAttendance(masterList);
        }

        public void ExcuseAbsentees(List<Attendance> attendance)
        {
            foreach (Attendance a in attendance)
            {
                _attendanceRepo.UpdateAttendance(a);
            }
        }
    }
}