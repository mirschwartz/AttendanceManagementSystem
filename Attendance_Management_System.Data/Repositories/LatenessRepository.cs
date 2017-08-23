using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Repositories
{
    public class LatenessRepository
    {
        private readonly string _connectionString;

        public LatenessRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Attendance> GetLatenesses()
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Attendances
                    .Include(a => a.StudentClass.Student)
                    .Where(a => a.Status == Status.UnexcusedLateness || a.Status == Status.ExcusedLateness)
                    .ToList();
            }
        }
    }
}