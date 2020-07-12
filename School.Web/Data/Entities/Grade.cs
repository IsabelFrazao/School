using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Grade
    {
        public int Id { get; set; }

        /*[Display(Name = "Subject")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Subject Subject { get; set; }

        [Display(Name = "Professor")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Professor Professor { get; set; }

        [Display(Name = "Student")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Student Student { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Course Course { get; set; }*/

        [Display(Name = "Class")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Class Class { get; set; }

        [Display(Name = "FinalGrade")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string FinalGrade { get; set; }

        [Display(Name = "Approval")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Approval { get; set; }
    }
}
