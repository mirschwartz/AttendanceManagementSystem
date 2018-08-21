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

        public void MarkAttendance(List<List<BCAttendance>> Class, List<BCTeacherSubject> teacher, DateTime date, int classId)
        {
            int index = 0;
            List<BCAttendance> masterList = new List<BCAttendance>();

            foreach (List<BCAttendance> attendance in Class)
            {
                int id = teacher[index].BCTeacherSubjectId;
                foreach (BCAttendance status in attendance)
                {
                    status.BCTeacherSubjectId = id;
                    status.Date = date;
                    masterList.Add(status);
                }
                index++;
            }

            masterList = masterList.Where(a => a.BCTeacherSubjectId != 0).ToList();

            _attendanceRepo.AddAttendance(masterList);
        }

        public void UpdateAttendance(DateTime date, int classId, List<List<BCAttendance>> attendance, List<BCTeacherSubject> teacher)
        {
            _attendanceRepo.DeleteDaysAttendance(date, classId);

            int index = 0;
            List<BCAttendance> masterList = new List<BCAttendance>();

            foreach (List<BCAttendance> a in attendance)
            {
                int id = teacher[index].BCTeacherSubjectId;

                foreach (BCAttendance status in a)
                {
                    status.BCTeacherSubjectId = id;
                    status.Date = date;
                    masterList.Add(status);
                }
                index++;
            }

            masterList = masterList.Where(a => a.BCTeacherSubjectId != 0).ToList();

            _attendanceRepo.AddAttendance(masterList);
        }

        public void ExcuseAbsentees(List<BCAttendance> attendance)
        {
            if(attendance != null)
            {
                foreach (BCAttendance a in attendance)
                {
                    _attendanceRepo.UpdateAttendance(a);
                }
            }
        }
    }
}