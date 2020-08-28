using School.Web.Data.Entities;
using School.Web.Models;

namespace School.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        #region STUDENT
        public Student ToStudent(StudentViewModel model, string path, bool isNew)//Converts to Model
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
                Schedule = model.Schedule,
                CourseId = model.Course.Id,
                Course = model.Course,
                ClassId = model.Class.Id,
                Class = model.Class,
                SchoolYear = model.SchoolYear
            };
        }

        public StudentViewModel ToStudentViewModel(Student model)//Converts to ViewModel
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
                CourseId = model.Course.Id,
                Course = model.Course,
                ClassId = model.Class.Id,
                Class = model.Class,
                SchoolYear = model.SchoolYear
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
                Email = model.Email
            };
        }

        public TeacherViewModel ToTeacherViewModel(Teacher model)//Converts to ViewModel
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
                Email = model.Email
            };
        }
        #endregion TEACHER

        #region CLASS
        public Class ToClass(ClassViewModel model, bool isNew)//Converts to Model
        {
            return new Class
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id
                Name = model.Name,
                Schedule = model.Schedule,
                Room = model.Room,
                CourseId = model.Course.Id,
                Course = model.Course
            };
        }

        public ClassViewModel ToClassViewModel(Class model)//Converts to ViewModel
        {
            return new ClassViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Schedule = model.Schedule,
                Room = model.Room,
                CourseId = model.Course.Id,
                Course = model.Course
            };
        }
        #endregion CLASS

        #region COURSE
        public Course ToCourse(CourseViewModel model, bool isNew)//Converts to Model
        {
            return new Course
            {
                Id = isNew ? 0 : model.Id, //Se o produto for novo => id = 0; senão apanhao o seu id
                Field = model.Field,
                Name = model.Name,
                Description = model.Description,
                CoordinatorId = model.Coordinator.Id,
                Coordinator = model.Coordinator,
                BeginDate = model.BeginDate.Date,
                EndDate = model.EndDate.Date
            };
        }

        public CourseViewModel ToCourseViewModel(Course model)//Converts to ViewModel
        {
            return new CourseViewModel
            {
                Id = model.Id,
                Field = model.Field,
                Name = model.Name,
                Description = model.Description,
                CoordinatorId = model.Coordinator.Id,
                Coordinator = model.Coordinator,
                BeginDate = model.BeginDate.Date,
                EndDate = model.EndDate.Date
            };
        }
        #endregion COURSE

        #region SUBJECT
        public Subject ToSubject(SubjectViewModel model, bool isNew)//Converts to Model
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
                CourseId = model.Course.Id,
                Course = model.Course,
                TeacherId = model.Teacher.Id,
                Teacher = model.Teacher
            };
        }

        public SubjectViewModel ToSubjectViewModel(Subject model)//Converts to ViewModel
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
                CourseId = model.Course.Id,
                Course = model.Course,
                TeacherId = model.Teacher.Id,
                Teacher = model.Teacher
            };
        }
        #endregion SUBJECT
    }
}
