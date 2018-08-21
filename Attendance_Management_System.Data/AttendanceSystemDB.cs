using Attendance_Management_System.Data.Models;
using MySql.Data.Entity;
using System.Data.Common;
using System.Data.Entity;

namespace Attendance_Management_System.Data
{
    internal class AttendanceSystemDBConfig : DbConfiguration
    {
        public AttendanceSystemDBConfig()
        {
            SetDatabaseInitializer<AttendanceSystemDB>(new NullDatabaseInitializer<AttendanceSystemDB>());
        }
    }

    [DbConfigurationType(typeof(AttendanceSystemDBConfig))]
    internal class AttendanceSystemDB : DbContext
    {
        public AttendanceSystemDB(string connectionString)
            : base(connectionString)
        {
        }

        public AttendanceSystemDB()
            : base("AttendanceSystemDB")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<BCAttendance> BCAttendances { get; set; }
        public DbSet<BCClass> BCClasses { get; set; }
        public DbSet<BCStudent> BCStudents { get; set; }
        public DbSet<BCStudentClass> BCStudentClasses { get; set; }
        public DbSet<BCTeacher> BCTeachers { get; set; }
        public DbSet<BCTeacherSubject> BCTeacherSubjects { get; set; }
        public DbSet<BCSchedule> BCSchedule { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Settings> Settings { get; set; }
    }
}
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    //public class AttendanceSystemDB : DbContext
    //{
    //    public AttendanceSystemDB(string connectionString)
    //        : base(connectionString)
    //    {
    //    }

    //    public AttendanceSystemDB()
    //        :base("AttendanceSystemDB")
    //    {
    //    }

    //    public DbSet<Attendance> Attendances { get; set; }
    //    public DbSet<Class> Classes { get; set; }
    //    public DbSet<Student> Students { get; set; }
    //    public DbSet<StudentClass> StudentClasses { get; set; }
    //    public DbSet<Teacher> Teachers { get; set; }
    //    public DbSet<TeacherSubject> TeacherSubjects { get; set; }
    //    public DbSet<User> Users { get; set; }
    //}
//}