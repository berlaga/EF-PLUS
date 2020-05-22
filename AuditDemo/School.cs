namespace AuditDemo
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }

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


            base.Seed(context);
        }
    }
}