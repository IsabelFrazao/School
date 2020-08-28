using Microsoft.AspNetCore.Http;
using School.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Models
{
    public class TeacherViewModel : Teacher
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Class> Classes { get; set; }        

        public IEnumerable<Subject> Subjects { get; set; }
    }
}