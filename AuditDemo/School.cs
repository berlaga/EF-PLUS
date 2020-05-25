namespace AuditDemo
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Z.EntityFramework.Plus;

    public class School : DbContext
    {
        // Your context has been configured to use a 'School' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'AuditDemo.School' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'School' 
        // connection string in the application configuration file.
        public School()
            : base("name=School")
        {
            Database.SetInitializer<School>(new SchoolDBIni<School>());

            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) => {
                var customAuditEntries = new List<CustomAudit>();

                foreach (var auditEntry in audit.Entries)
                {
                    var customAuditEntry = new CustomAudit();
                    customAuditEntries.Add(customAuditEntry);

                    customAuditEntry.EntitySetName = auditEntry.EntitySetName;
                    customAuditEntry.EntityTypeName = auditEntry.EntityTypeName;
                    customAuditEntry.StateName = auditEntry.StateName;
                    customAuditEntry.CreatedBy = auditEntry.CreatedBy;
                    customAuditEntry.CreatedDate = auditEntry.CreatedDate;

                    if(auditEntry.EntityTypeName == "Enrollment")
                    {
                        var record = ((School)context).SchoolStatistics.Find(1);
                        record.EnrolmentCount++;
                    }

                    if (auditEntry.EntityTypeName == "Student")
                    {
                        var record = ((School)context).SchoolStatistics.Find(1);
                        record.StudentCount++;
                    }

                }

                 ((School)context).CustomAudits.AddRange(customAuditEntries);
            };
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<CustomAudit> CustomAudits { get; set; }
        public virtual DbSet<SchoolStatistic> SchoolStatistics { get; set; }

    }

    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class Enrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }

    }

    public class SchoolStatistic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StudentCount { get; set; }
        public int EnrolmentCount { get; set; }
    }

    public class CustomAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EntitySetName { get; set; }
        public string EntityTypeName { get; set; }
        public string StateName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class SchoolDBIni<T> : DropCreateDatabaseAlways<School>
    {

        protected override void Seed(School context)
        {

            IList<Student> students = new List<Student>();

            students.Add(new Student()
            {
                FirstName = "Andrew",
                LastName = "Peters",
            });

            students.Add(new Student()
            {
                FirstName = "Brice",
                LastName = "Lambson",
            });

            students.Add(new Student()
            {
                FirstName = "Rowan",
                LastName = "Miller",
            });

            foreach (Student student in students)
                context.Students.Add(student);


            IList<Course> courses = new List<Course>();

            courses.Add(new Course()
            {
                Description = "Math course",
                Name = "Math Grade 10"
            });

            courses.Add(new Course()
            {
                Description = "Math course",
                Name = "Math Grade 11"
            });

            courses.Add(new Course()
            {
                Description = "Math course",
                Name = "Math Grade 12"
            });

            foreach (Course c in courses)
                context.Courses.Add(c);

            context.SchoolStatistics.Add(new SchoolStatistic() {
                StudentCount = 3,
                EnrolmentCount = 0 });

            base.Seed(context);
        }
    }
}