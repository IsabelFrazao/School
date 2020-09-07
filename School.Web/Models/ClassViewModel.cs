using School.Web.Data.Entities;
using System.Collections.Generic;

namespace School.Web.Models
{
    public class ClassViewModel : Class
    {
        public IEnumerable<Course> Courses { get; set; }

        public string[] Schedules = new[] { "Day", "Night" };

        public string[] Classrooms = new[] { "3.01", "3.02" };

        public IEnumerable<Student> Students { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }
    }
}
