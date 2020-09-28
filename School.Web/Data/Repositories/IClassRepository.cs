using School.Web.Data.Entities;
using School.Web.Models;
using System.Collections.Generic;

namespace School.Web.Data.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        IEnumerable<Teacher> GetTeachers(ClassViewModel model, IEnumerable<Teacher> teachers, IEnumerable<Subject> subjects);
    }
}
