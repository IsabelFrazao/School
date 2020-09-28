using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Student : Person
    {
        public Schedule Schedule { get; set; }

        [Display(Name = "Schedule")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int ScheduleId { get; set; }
                
        public Course Course { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int CourseId { get; set; }
        
        public Class Class { get; set; }

        [Display(Name = "Class")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int ClassId { get; set; }

        [Display(Name = "School Year")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string SchoolYear { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FullName}";
        }
    }
}
