using Microsoft.AspNetCore.Mvc.Rendering;
using School.Web.Controllers;
using School.Web.Data.Entities;
using School.Web.Models;
using System.Collections.Generic;

namespace School.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        #region STUDENT
        public Student ToStudent(StudentViewModel model, string path, Course course, Class classes, bool isNew)//Converts to Model
        {
            return new Student
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id
                PhotoUrl = path,
                FullName = model.FullName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                ZipCode = model.ZipCode,
                City = model.City,
                IdentificationNumber = model.IdentificationNumber,
                TaxNumber = model.TaxNumber,
                SSNumber = model.SSNumber,
                NHSNumber = model.NHSNumber,
                MaritalStatus = model.MaritalStatus,
                Nationality = model.Nationality,
                Telephone = model.Telephone,
                Email = model.Email,
                ScheduleId = model.ScheduleId,
                Course = course,
                CourseId = model.CourseId,
                Class = classes,
                ClassId = model.ClassId,                
                SchoolYear = model.SchoolYear,
                isActive = true,
                User = model.User
            };
        }

        public StudentViewModel ToStudentViewModel(Student model, Course course, IEnumerable<Course> courses, Class classes, IEnumerable<Class> listclasses)//Converts to ViewModel
        {
            return new StudentViewModel
            {
                Id = model.Id,
                PhotoUrl = model.PhotoUrl,
                FullName = model.FullName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                ZipCode = model.ZipCode,
                City = model.City,
                IdentificationNumber = model.IdentificationNumber,
                TaxNumber = model.TaxNumber,
                SSNumber = model.SSNumber,
                NHSNumber = model.NHSNumber,
                MaritalStatus = model.MaritalStatus,
                Nationality = model.Nationality,
                Telephone = model.Telephone,
                Email = model.Email,
                Schedule = model.Schedule,
                Course = course,
                CourseId = model.CourseId,                
                Courses = courses,
                Class = classes,
                ClassId = model.ClassId,                
                Classes = listclasses,
                SchoolYear = model.SchoolYear,
                isActive = true,
                User = model.User
            };
        }
        #endregion STUDENT

        #region TEACHER
        public Teacher ToTeacher(TeacherViewModel model, string path, bool isNew)
        {
            return new Teacher
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id
                PhotoUrl = path,
                FullName = model.FullName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                ZipCode = model.ZipCode,
                City = model.City,
                IdentificationNumber = model.IdentificationNumber,
                TaxNumber = model.TaxNumber,
                SSNumber = model.SSNumber,
                NHSNumber = model.NHSNumber,
                MaritalStatus = model.MaritalStatus,
                Nationality = model.Nationality,
                Telephone = model.Telephone,
                Email = model.Email,
                isActive = true,
                User = model.User
            };
        }

        public TeacherViewModel ToTeacherViewModel(Teacher model, IEnumerable<Subject> subjects)//Converts to ViewModel
        {
            return new TeacherViewModel
            {
                Id = model.Id,
                PhotoUrl = model.PhotoUrl,
                FullName = model.FullName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                ZipCode = model.ZipCode,
                City = model.City,
                IdentificationNumber = model.IdentificationNumber,
                TaxNumber = model.TaxNumber,
                SSNumber = model.SSNumber,
                NHSNumber = model.NHSNumber,
                MaritalStatus = model.MaritalStatus,
                Nationality = model.Nationality,
                Telephone = model.Telephone,
                Email = model.Email,
                Subjects = subjects,
                isActive = true,
                User = model.User
            };
        }
        #endregion TEACHER

        #region CLASS
        public Class ToClass(ClassViewModel model, Course course, bool isNew)//Converts to Model
        {
            return new Class
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id
                Name = model.Name,
                ScheduleId = model.ScheduleId,
                ClassroomId = model.ClassroomId,
                CourseId = course.Id,
                Course = course,
                isActive = true
            };
        }

        public ClassViewModel ToClassViewModel(Class model, Course course, IEnumerable<Student> students, IEnumerable<Subject> subjects)//Converts to ViewModel
        {
            return new ClassViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Schedule = model.Schedule,
                Classroom = model.Classroom,
                CourseId = model.CourseId,
                Course = course,
                Students = students,
                Subjects = subjects,
                isActive = true
            };
        }
        #endregion CLASS

        #region COURSE
        public Course ToCourse(CourseViewModel model, bool isNew)//Converts to Model
        {
            return new Course
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id
                FieldId = model.FieldId,
                Name = model.Name,
                Description = model.Description,
                CoordinatorId = model.CoordinatorId,
                Coordinator = model.Coordinator,
                BeginDate = model.BeginDate.Date,
                EndDate = model.EndDate.Date,
                SchoolYear = model.SchoolYear = $"{ model.BeginDate.Year } / { model.EndDate.Year }",
                isActive = true
            };
        }

        public CourseViewModel ToCourseViewModel(Course model, Teacher coordinator, IEnumerable<Teacher> teachers, IEnumerable<Class> classes, IEnumerable<Subject> subjects)//Converts to ViewModel
        {
            return new CourseViewModel
            {
                Id = model.Id,
                FieldId = model.FieldId,
                Name = model.Name,
                Description = model.Description,
                CoordinatorId = model.CoordinatorId,
                Coordinator = coordinator,
                BeginDate = model.BeginDate.Date,
                EndDate = model.EndDate.Date,
                SchoolYear = model.SchoolYear = $"{ model.BeginDate.Year } / { model.EndDate.Year }",
                Teachers = teachers,
                Classes = classes,
                Subjects = subjects,
                isActive = true
            };
        }
        #endregion COURSE

        #region SUBJECT
        public Subject ToSubject(SubjectViewModel model, Course course, Teacher teacher, bool isNew)//Converts to Model
        {
            return new Subject
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id
                Code = model.Code,
                Name = model.Name,
                Duration = model.Duration,
                Credits = model.Credits,
                ReferenceCode = model.ReferenceCode,
                FieldCode = model.FieldCode,
                Field = model.Field,
                Reference = model.Reference,
                QNQLevel = model.QNQLevel,
                QEQLevel = model.QEQLevel,
                Component = model.Component,
                CourseId = model.CourseId,
                Course = model.Course,
                TeacherId = model.TeacherId,
                Teacher = model.Teacher,
                isActive = true
            };
        }

        public SubjectViewModel ToSubjectViewModel(Subject model, IEnumerable<Course> courses, IEnumerable<Teacher> teachers)//Converts to ViewModel
        {
            return new SubjectViewModel
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                Duration = model.Duration,
                Credits = model.Credits,
                ReferenceCode = model.ReferenceCode,
                FieldCode = model.FieldCode,
                Field = model.Field,
                Reference = model.Reference,
                QNQLevel = model.QNQLevel,
                QEQLevel = model.QEQLevel,
                Component = model.Component,
                CourseId = model.CourseId,
                Course = model.Course,
                TeacherId = model.TeacherId,
                Teacher = model.Teacher,
                Courses = courses,
                Teachers = teachers,
                isActive = true
            };
        }

        public Subject ConvertToSubject(IEFPSubject model, int courseid, Course course, int teacherid, Teacher teacher, string credits, bool isNew)
        {
            return new Subject
            {
                Id = isNew ? 0 : model.Id,
                Code = model.Code,
                Name = model.Name,
                Duration = model.Duration,
                Credits = credits,
                ReferenceCode = model.ReferenceCode,
                FieldCode = model.FieldCode,
                Field = model.Field,
                Reference = model.Reference,
                QNQLevel = model.QNQLevel,
                QEQLevel = model.QEQLevel,
                Component = model.Component,
                CourseId = courseid,
                Course = course,
                TeacherId = teacherid,
                Teacher = teacher,
                isActive = true
            };
        }
        #endregion SUBJECT

        #region GRADE
        public Grade ToGrade(GradeViewModel model, Course course, Class classes, Subject subject, Teacher teacher, Student student, bool isNew)//Converts to Model
        {
            return new Grade
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id                
                Course = course,
                CourseId = model.CourseId,
                Class = classes,
                ClassId = model.ClassId,
                SubjectId = model.SubjectId,
                Subject = subject,
                TeacherId = model.TeacherId,
                Teacher = teacher,
                StudentId = model.StudentId,
                Student = student,
                FinalGrade = model.FinalGrade,
                Approval = model.FinalGrade >= 10 ? model.Approval = "Approved" : model.Approval = "Disapproved",
                isActive = true
            };
        }

        public GradeViewModel ToGradeViewModel(Grade model, Course course, IEnumerable<Course> courses, Class classes, IEnumerable<Class> listclasses, IEnumerable<Subject> subjects, IEnumerable<Teacher> teachers, IEnumerable<Student> students)//Converts to ViewModel
        {
            return new GradeViewModel
            {
                Id = model.Id,
                Course = course,
                CourseId = model.CourseId,
                Courses = courses,
                Class = classes,
                ClassId = model.ClassId,
                Classes = listclasses,
                SubjectId = model.SubjectId,
                Subject = model.Subject,
                Subjects = subjects,
                TeacherId = model.Subject.TeacherId,
                Teacher = model.Subject.Teacher,
                Teachers = teachers,
                StudentId = model.StudentId,
                Student = model.Student,
                Students = students,
                FinalGrade = model.FinalGrade,
                Approval = model.FinalGrade >= 10 ? model.Approval = "Approved" : model.Approval = "Disapproved",
                isActive = true
            };
        }
        #endregion GRADE
    }
}
