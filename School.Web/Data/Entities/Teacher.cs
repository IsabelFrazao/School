using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Entities
{
    public class Teacher : Person
    {
        //    [Display(Name = "Subject")]
        //    //[Required(ErrorMessage = "Field {0} is mandatory")]
        //    //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        //    public Subject Subject { get; set; }

        //    public int SubjectId { get; set; }

        //public List<Nota> Notas { get; set; }

        public override string ToString()
        {
            return $"{FullName}";
        }
    }
}
