using Attendance_Management_System.Data.Models;
using Attendance_Management_System.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Services
{
    public class StudentService
    {
        private readonly StudentRepository _StudentRepo;

        public StudentService()
        {

        }

        public StudentService(string connectionString)
        {
            _StudentRepo = new StudentRepository(connectionString);
        }
    }
}