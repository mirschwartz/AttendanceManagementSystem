using Attendance_Management_System.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _teacherRepo;

        public TeacherService()
        {

        }

        public TeacherService(string connectionString)
        {
            _teacherRepo = new TeacherRepository(connectionString);
        }
    }
}
