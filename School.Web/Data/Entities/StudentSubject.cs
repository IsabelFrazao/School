using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class StudentSubject : IEntity
    {
        public int Id { get; set; }

        //[Display(Name = "Student")]
        //////[Required(ErrorMessage = "Field {0} is mandatory")]
        //////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        //public Student Student { get; set; }

        public int StudentId { get; set; }

        //[Display(Name = "Subject")]
        //////[Required(ErrorMessage = "Field {0} is mandatory")]
        //////[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        //public Subject Subject { get; set; }

        public int SubjectId { get; set; }
    }
}
