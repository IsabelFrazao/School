using System.ComponentModel.DataAnnotations;

namespace School.Web.Data.Entities
{
    public class Grade : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Course")]
        public Course Course { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "Class")]
        public Class Class { get; set; }

        public int ClassId { get; set; }

        [Display(Name = "Subject")]
        public Subject Subject { get; set; }

        public int SubjectId { get; set; }

        [Display(Name = "Teacher")]
        public Teacher Teacher { get; set; }

        public int TeacherId { get; set; }

        [Display(Name = "Student")]
        public Student Student { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "FinalGrade")]
        public decimal FinalGrade { get; set; }

        [Display(Name = "Approval")]
        public string Approval { get; set; }

        public bool isActive { get; set; }
    }
}
