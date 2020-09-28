using School.Web.Data.Entities;
using System.Collections.Generic;

namespace School.Web.Models
{
    public class CourseViewModel : Course
    {
        public IEnumerable<Field> Fields { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }
    }
}
