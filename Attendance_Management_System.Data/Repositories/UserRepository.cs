using Attendance_Management_System.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Data.Repositories
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user)
        { 
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
        }

        public User GetUser(string userName)
        {
            using (var dbContext = new AttendanceSystemDB(_connectionString))
            {
                return dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            }
        }
    }
}