using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Subject : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Code { get; set; }

        [Display(Name = "Name")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Duration")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Duration { get; set; }

        [Display(Name = "Credits")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Credits { get; set; }

        [Display(Name = "Reference Code")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string ReferenceCode { get; set; }

        [Display(Name = "Field Code")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string FieldCode { get; set; }

        [Display(Name = "Field")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Field { get; set; }

        [Display(Name = "Reference")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Reference { get; set; }

        [Display(Name = "QNQ Level")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string QNQLevel { get; set; }

        [Display(Name = "QEQ Level")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string QEQLevel { get; set; }

        [Display(Name = "Component")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Component { get; set; }

        public string WebCode { get; set; }

        [Display(Name = "Course")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Course Course { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "Teacher")]
        //[Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Teacher Teacher { get; set; }

        public int TeacherId { get; set; }

        //public Class Class { get; set; }

        //public Student Student { get; set; }

        //public Grade Grade { get; set; }

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }
    }
}
