using Attendance_Management_System.Data.Models;
using Attendance_Management_System.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService()
        {

        }

        public UserService(string connectionString)
        {
            _userRepo = new UserRepository(connectionString);
        }

        public void AddUser(string schoolName, string userName, string subdomain, string password)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);

            User user = new User
            {
                SchoolName = schoolName,
                UserName = userName,
                SubDomain = subdomain,
                PasswordHash = hash,
                Salt = salt
            };
            _userRepo.AddUser(user);
        }

        public User Login(string userName, string password)
        {
            User user = _userRepo.GetUser(userName);

            if(user == null)
            {
                return null;
            }

            bool isMatch = PasswordHelper.IsMatch(password, user.PasswordHash, user.Salt);
            if (isMatch)
            {
                return user;
            }

            return null;
        } 
    }
}