using Microsoft.AspNetCore.Mvc.Rendering;
using School.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace School.Web.Data.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(DataContext context) : base(context)
        {
        }        
    }
}
