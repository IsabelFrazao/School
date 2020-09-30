using Microsoft.AspNetCore.Http;
using School.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Models
{
    public class SubjectViewModel : Subject
    {
        [Display(Name = "Image")]
        public IFormFile UploadFile { get; set; }

        public ICollection<IEFPSubject> IEFPSubjects { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}
