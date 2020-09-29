using System.ComponentModel.DataAnnotations;

namespace School.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string Confirm { get; set; }

        public string UserId { get; set; }
    }
}
