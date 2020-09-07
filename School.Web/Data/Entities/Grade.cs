using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Grade : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Course")]
        ////[Required(ErrorMessage = "Field {0} is mandatory")]
        ////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Course Course { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "Class")]
        ////[Required(ErrorMessage = "Field {0} is mandatory")]
        ////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Class Class { get; set; }

        public int ClassId { get; set; }

        [Display(Name = "Subject")]
        ////[Required(ErrorMessage = "Field {0} is mandatory")]
        ////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Subject Subject { get; set; }

        public int SubjectId { get; set; }

        [Display(Name = "Teacher")]
        ////[Required(ErrorMessage = "Field {0} is mandatory")]
        ////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Teacher Teacher { get; set; }

        public int TeacherId { get; set; }

        [Display(Name = "Student")]
        ////[Required(ErrorMessage = "Field {0} is mandatory")]
        ////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Student Student { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "FinalGrade")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public decimal FinalGrade { get; set; }

        [Display(Name = "Approval")]
        ////[Required(ErrorMessage = "Field {0} is mandatory")]
        ////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Approval { get; set; }
    }
}
