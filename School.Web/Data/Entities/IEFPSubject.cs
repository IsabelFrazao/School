using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class IEFPSubject : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Duration")]
        public string Duration { get; set; }

        [Display(Name = "Credits")]
        public string Credits { get; set; }

        [Display(Name = "Reference Code")]
        public string ReferenceCode { get; set; }

        [Display(Name = "Field Code")]
        public string FieldCode { get; set; }

        [Display(Name = "Field")]
        public string Field { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Display(Name = "QNQ Level")]
        public string QNQLevel { get; set; }

        [Display(Name = "QEQ Level")]
        public string QEQLevel { get; set; }

        [Display(Name = "Component")]
        public string Component { get; set; }

        public bool isActive { get; set; }

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }
    }
}
