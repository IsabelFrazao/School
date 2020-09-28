using School.Web.Data.Entities;
using School.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Teacher> GetTeachers(ClassViewModel model, IEnumerable<Teacher> teachers, IEnumerable<Subject> subjects)
        {
            model.Teachers = new List<Teacher>();

            List<Teacher> Teachers = teachers.ToList();

            var Subjects = new List<Subject>(subjects);

            int rep = 0;

            foreach (var subject in Subjects)
            {
                foreach (var teacher in Teachers)
                {
                    if (teacher.Id == subject.TeacherId && teacher.Id != rep)
                    {
                        model.Teachers.Add(new Teacher
                        {
                            Id = teacher.Id,
                            PhotoUrl = teacher.PhotoUrl,
                            FullName = teacher.FullName,
                            Gender = teacher.Gender,
                            DateOfBirth = teacher.DateOfBirth,
                            Address = teacher.Address,
                            ZipCode = teacher.ZipCode,
                            City = teacher.City,
                            IdentificationNumber = teacher.IdentificationNumber,
                            TaxNumber = teacher.TaxNumber,
                            SSNumber = teacher.SSNumber,
                            NHSNumber = teacher.NHSNumber,
                            MaritalStatus = teacher.MaritalStatus,
                            Nationality = teacher.Nationality,
                            Telephone = teacher.Telephone,
                            Email = teacher.Email
                        });

                        rep = teacher.Id;
                    }
                }
            }
            return model.Teachers;
        }
    }
}
