using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Student : Person
    {
        [Display(Name = "Schedule")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Schedule { get; set; }

        [Display(Name = "Course")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Course Course { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "Class")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Class Class { get; set; }

        public int ClassId { get; set; }

        public string SchoolYear { get; set; }

        //CLASS
        //SCHOOL YEAR

        public override string ToString()
        {
            return $"{Id} - {FullName}";
        }
    }
}
