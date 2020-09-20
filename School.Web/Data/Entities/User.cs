using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Entities
{
    public class User : IdentityUser
    {
        //[MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string FirstName { get; set; }

        //[MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string LastName { get; set; }

        public bool isActive { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
