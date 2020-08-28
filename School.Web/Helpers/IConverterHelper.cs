using School.Web.Data.Entities;
using School.Web.Models;

namespace School.Web.Helpers
{
    public interface IConverterHelper
    {
        Student ToStudent(StudentViewModel model, string path, bool isNew);
        StudentViewModel ToStudentViewModel(Student model);

        Teacher ToTeacher(TeacherViewModel model, string path, bool isNew);
        TeacherViewModel ToTeacherViewModel(Teacher model);

        Class ToClass(ClassViewModel model, bool isNew);
        ClassViewModel ToClassViewModel(Class model);

        Course ToCourse(CourseViewModel model, bool isNew);
        CourseViewModel ToCourseViewModel(Course model);

        Subject ToSubject(SubjectViewModel model, bool isNew);
        SubjectViewModel ToSubjectViewModel(Subject model);
    }
}