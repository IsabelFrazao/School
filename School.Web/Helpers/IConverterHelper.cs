using School.Web.Data.Entities;
using School.Web.Models;
using System.Collections.Generic;

namespace School.Web.Helpers
{
    public interface IConverterHelper
    {
        Student ToStudent(StudentViewModel model, string path, Course course, Class classes, bool isNew);
        StudentViewModel ToStudentViewModel(Student model, Course course, IEnumerable<Course> courses, Class classes, IEnumerable<Class> listclasses);

        Teacher ToTeacher(TeacherViewModel model, string path, bool isNew);
        TeacherViewModel ToTeacherViewModel(Teacher model, IEnumerable<Subject> subjects);

        Class ToClass(ClassViewModel model, Course course, bool isNew);
        ClassViewModel ToClassViewModel(Class model, Course course, IEnumerable<Student> students, IEnumerable<Subject> subjects);

        Course ToCourse(CourseViewModel model, bool isNew);
        CourseViewModel ToCourseViewModel(Course model, Teacher coordinator, IEnumerable<Teacher> teachers, IEnumerable<Class> classes, IEnumerable<Subject> subjects);

        Subject ToSubject(SubjectViewModel model, Course course, Teacher teacher, bool isNew);
        SubjectViewModel ToSubjectViewModel(Subject model, IEnumerable<Course> courses, IEnumerable<Teacher> teachers);
        Subject ConvertToSubject(IEFPSubject model, int courseid, Course course, int teacherid, Teacher teacher, string credits, bool isNew);

        Grade ToGrade(GradeViewModel model, Course course, Class classes, Subject subject, Teacher teacher, Student student, bool isNew);
        GradeViewModel ToGradeViewModel(Grade model, Course course, IEnumerable<Course> courses, Class classes, IEnumerable<Class> listclasses, IEnumerable<Subject> subjects, IEnumerable<Teacher> teachers, IEnumerable<Student> students);
    }
}