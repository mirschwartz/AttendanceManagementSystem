using Attendance_Management_System.Helpers;
using Attendance_Management_System.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSignup
{
    class Program
    {
        static void Main(string[] args)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["AttendanceSystemDB"].ConnectionString;

            Console.WriteLine("Enter school name:");
            var schoolName = Console.ReadLine();
            Console.WriteLine("Enter User Name:");
            var userName = Console.ReadLine();
            Console.WriteLine("Enter Subdomain Name(optional):");
            var subdomain = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            var password = Console.ReadLine();

            var userservice = new UserService(_connectionString);
            userservice.AddUser(schoolName, userName, subdomain, password);

            Console.ReadKey(true);
        }
    }
}
