using School.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Models
{
    public class SubjectViewModel : Subject
    {
        public IEnumerable<IEFPSubject> IEFPSubjects { get; set; }

        public string[] Fields = new[] { "Áudiovisuais e Produção dos Media", "Ciências Informáticas", "Eletrónica e Automação" };

        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }  
    }
}
