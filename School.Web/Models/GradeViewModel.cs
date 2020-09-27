﻿using School.Web.Data.Entities;
using System.Collections.Generic;

namespace School.Web.Models
{
    public class GradeViewModel : Grade
    {
        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }

        public IEnumerable<Student> Students { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
