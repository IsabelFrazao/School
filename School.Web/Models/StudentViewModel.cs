using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IEnumerable<Schedule> Schedules { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public List<string> SchoolYears { get; set; }

        public IEnumerable<Grade> Grades { get; set; }

        public IEnumerable<SelectListItem> ComboCourse { get; set; }
    }
}
