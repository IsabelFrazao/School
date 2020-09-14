using Microsoft.AspNetCore.Mvc.Rendering;
using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<bool> ValidationAsync(string identificationNumber, string taxNumber, string ssNumber, string nhsNumber,
            string telephone, string email);
    }
}
