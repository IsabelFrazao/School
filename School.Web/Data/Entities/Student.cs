using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Student : Person
    {
        [Display(Name = "Schedule")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Schedule { get; set; }

        [Display(Name = "Course")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Course Course { get; set; }

        public int CourseId { get; set; }

        
        public Class Class { get; set; }

        [Display(Name = "Class")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int ClassId { get; set; }

        [Display(Name = "School Year")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string SchoolYear { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FullName}";
        }
    }
}
