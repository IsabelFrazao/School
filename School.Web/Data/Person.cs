using System;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Data
{
    public class Person
    {
        public int Id { get; set; }

        //[Display(Name = "Photo")]
        //public string PhotoUrl { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string FullName { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Address { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string ZipCode { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(50, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string City { get; set; }

        [Display(Name = "Identification Number")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string IdentificationNumber { get; set; }

        [Display(Name = "Tax Number")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string TaxNumber { get; set; }

        [Display(Name = "Social Security Number")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string SSNumber { get; set; }

        [Display(Name = "National Health Service Number")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string NHSNumber { get; set; }

        [Display(Name = "Marital Status")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        //[Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Nationality")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string Nationality { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telephone Number")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 9)]
        public string Telephone { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(30, ErrorMessage = "Field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Email { get; set; }

        //public Person(int id, /*string photoURL, */string fullName, string gender, DateTime dateOfBirth, string address, string zipCode, string city,
        //    string identificationNumber, string taxNumber, string sSNumber, string nHSNumber, string maritalStatus, string nationality,
        //    string telephone, string email)
        //{
        //    Id = id;
        //    //PhotoURL = photoURL;
        //    FullName = fullName;
        //    Gender = gender;
        //    DateOfBirth = dateOfBirth;
        //    Address = address;
        //    ZipCode = zipCode;
        //    City = city;
        //    IdentificationNumber = identificationNumber;
        //    TaxNumber = taxNumber;
        //    SSNumber = sSNumber;
        //    NHSNumber = nHSNumber;
        //    MaritalStatus = maritalStatus;
        //    Nationality = nationality;
        //    Telephone = telephone;
        //    Email = email;
        //}
    }
}
