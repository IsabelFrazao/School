using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<bool> ExistsCodeAsync(string code);
    }
}
