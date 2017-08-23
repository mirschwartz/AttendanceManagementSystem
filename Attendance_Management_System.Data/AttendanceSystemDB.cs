using Attendance_Management_System.Data.Models;
using MySql.Data.Entity;
using System.Data.Common;
using System.Data.Entity;

namespace Attendance_Management_System.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class AttendanceSystemDB : DbContext
    {
        public AttendanceSystemDB(string connectionString)
            : base(connectionString)
        {
        }

        public AttendanceSystemDB()
            :base("AttendanceSystemDB")
        {
        }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
