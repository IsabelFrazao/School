using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Entities
{
    public class Professor : Person
    {
        public Professor(int id, string fullName, string gender, DateTime dateOfBirth, string address, string zipCode, string city,
            string identificationNumber, string taxNumber, string sSNumber, string nHSNumber, string maritalStatus, string nationality,
            string telephone, string email)
            : base(id, fullName, gender, dateOfBirth, address, zipCode, city, identificationNumber, taxNumber, sSNumber, nHSNumber, maritalStatus,
                  nationality, telephone, email)
        {
        }

        /*[Display(Name = "Subject")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public Subject Subject { get; set; }*/
        //public List<Nota> Notas { get; set; }

        public override string ToString()
        {
            return $"{FullName}";
        }
    }
}
