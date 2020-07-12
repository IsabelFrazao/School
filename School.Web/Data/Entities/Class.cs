using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Class
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field {0} is mandatory")]
        public string Name { get; set; }

        [Display(Name = "Schedule")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Schedule { get; set; }

        [Display(Name = "Room")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Room { get; set; }

        /*[Display(Name = "Course")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Course Course { get; set; }*/

        //public UFCD UFCDdaTurma { get; set; }

        //public Student Student { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
