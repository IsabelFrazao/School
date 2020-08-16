using System;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Models
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Field { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /*[Display(Name = "Coordinator")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Professor Coordinator { get; set; }*/

        [DataType(DataType.Date)]
        [Display(Name = "Begin Date")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime BeginDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        //Turma
        //UFCD
        //public virtual ICollection<Student> Student { get; set; }
    }
}
