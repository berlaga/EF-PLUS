using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var context = new School())
            {
                Console.Write(context.Students.Count());
                context.SaveChanges();
            }
        }
    }
}
