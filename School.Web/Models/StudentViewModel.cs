using Microsoft.AspNetCore.Http;
using School.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Models
{
    public class StudentViewModel : Student
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public string[] Schedules = new[] { "Day", "Night" };

        public IEnumerable<Class> Classes { get; set; }

        public List<string> SchoolYears = new List<string>();

        public IEnumerable<Grade> Grades { get; set; }
    }
}
