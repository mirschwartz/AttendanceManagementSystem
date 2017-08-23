using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Attendance_Management_System.Helpers
{
    public class ConnectionString
    {
        private static string _connectionString = null;

        public static string DB
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["AttendanceSystemDB"].ConnectionString;
                }

                return _connectionString;
            }
        }
    }
}