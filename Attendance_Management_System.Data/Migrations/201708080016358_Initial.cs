namespace Attendance_Management_System.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendanceId = c.Int(nullable: false, identity: true),
                        TeacherSubjectId = c.Int(nullable: false),
                        StudentClassId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Period = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttendanceId)
                .ForeignKey("dbo.StudentClasses", t => t.StudentClassId, cascadeDelete: true)
                .ForeignKey("dbo.TeacherSubjects", t => t.TeacherSubjectId, cascadeDelete: true)
                .Index(t => t.TeacherSubjectId)
                .Index(t => t.StudentClassId);
            
            CreateTable(
                "dbo.StudentClasses",
                c => new
                    {
                        StudentClassId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StudentClassId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        ClassName = c.String(nullable: false, unicode: false),
                        Year = c.String(unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClassId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, unicode: false),
                        LastName = c.String(nullable: false, unicode: false),
                        Address1 = c.String(unicode: false),
                        Address2 = c.String(unicode: false),
                        City = c.String(unicode: false),
                        State = c.String(unicode: false),
                        Zip = c.String(maxLength: 10, storeType: "nvarchar"),
                        HomePhone = c.String(maxLength: 15, storeType: "nvarchar"),
                        CellPhone = c.String(maxLength: 15, storeType: "nvarchar"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "dbo.TeacherSubjects",
                c => new
                    {
                        TeacherSubjectId = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(nullable: false),
                        Subject = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.TeacherSubjectId)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherId = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(nullable: false, unicode: false),
                        HomePhone = c.String(maxLength: 15, storeType: "nvarchar"),
                        CellPhone = c.String(maxLength: 15, storeType: "nvarchar"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        SchoolName = c.String(unicode: false),
                        UserName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        SubDomain = c.String(unicode: false),
                        PasswordHash = c.String(nullable: false, unicode: false),
                        Salt = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeacherSubjects", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Attendances", "TeacherSubjectId", "dbo.TeacherSubjects");
            DropForeignKey("dbo.Attendances", "StudentClassId", "dbo.StudentClasses");
            DropForeignKey("dbo.StudentClasses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentClasses", "ClassId", "dbo.Classes");
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.TeacherSubjects", new[] { "TeacherId" });
            DropIndex("dbo.StudentClasses", new[] { "ClassId" });
            DropIndex("dbo.StudentClasses", new[] { "StudentId" });
            DropIndex("dbo.Attendances", new[] { "StudentClassId" });
            DropIndex("dbo.Attendances", new[] { "TeacherSubjectId" });
            DropTable("dbo.Users");
            DropTable("dbo.Teachers");
            DropTable("dbo.TeacherSubjects");
            DropTable("dbo.Students");
            DropTable("dbo.Classes");
            DropTable("dbo.StudentClasses");
            DropTable("dbo.Attendances");
        }
    }
}
