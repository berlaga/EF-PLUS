using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace AuditDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var context = new School())
            {

                var newStudent = context.Students.Add(new Student()
                {
                    FirstName = "New",
                    LastName ="Student"
                });

                context.Enrollments.Add(new Enrollment()
                {
                    Course = context.Courses.First(),
                    Student = newStudent,
                    EnrollmentDate = DateTime.Now
                });

                var audit = new Audit();
                audit.CreatedBy = "ZZZ Projects"; // Optional
                context.SaveChanges(audit);

                
            }
        }
    }
}
