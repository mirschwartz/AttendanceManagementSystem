namespace Attendance_Management_System.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatenessesAndSubjectActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "SigningIn", c => c.Boolean());
            AddColumn("dbo.Attendances", "notes", c => c.String(unicode: false));
            AddColumn("dbo.TeacherSubjects", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeacherSubjects", "IsActive");
            DropColumn("dbo.Attendances", "notes");
            DropColumn("dbo.Attendances", "SigningIn");
        }
    }
}
