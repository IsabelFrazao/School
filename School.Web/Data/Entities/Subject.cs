using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Subject : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Duration")]
        public string Duration { get; set; }

        [Display(Name = "Credits")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        public string Credits { get; set; }

        [Display(Name = "Reference Code")]
        public string ReferenceCode { get; set; }

        [Display(Name = "Field Code")]
        public string FieldCode { get; set; }

        [Display(Name = "Field")]
        public string Field { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Display(Name = "QNQ Level")]
        public string QNQLevel { get; set; }

        [Display(Name = "QEQ Level")]
        public string QEQLevel { get; set; }

        [Display(Name = "Component")]
        public string Component { get; set; }
                
        public Course Course { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int CourseId { get; set; }
                
        public Teacher Teacher { get; set; }

        [Display(Name = "Teacher")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int TeacherId { get; set; }

        public bool isActive { get; set; }

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }
    }
}
