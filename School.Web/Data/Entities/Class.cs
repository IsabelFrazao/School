using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Class : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field {0} is mandatory")]
        public string Name { get; set; }
                
        public Schedule Schedule { get; set; }

        [Display(Name = "Schedule")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int ScheduleId { get; set; }

        public Classroom Classroom { get; set; }

        [Display(Name = "Classroom")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int ClassroomId { get; set; }

        public Course Course { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public int CourseId { get;set; }

        public bool isActive { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
