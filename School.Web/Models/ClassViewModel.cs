using School.Web.Data.Entities;
using System.Collections.Generic;

namespace School.Web.Models
{
    public class ClassViewModel : Class
    {
        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Schedule> Schedules { get; set; }

        public IEnumerable<Classroom> Classrooms { get; set; }

        public IEnumerable<Student> Students { get; set; }

        public ICollection<Teacher> Teachers { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }
    }
}
