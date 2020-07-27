using System;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Student : Person
    {        
        [Display(Name = "Schedule")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string Schedule { get; set; }

        /*[Display(Name = "Course")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Course Course { get; set; }*/

        //public virtual Course Course { get; set; }

        //public virtual Class Class { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FullName}";
        }
    }
}
