using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly DataContext _context;

        public SubjectRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsCodeAsync(string code)
        {
            return await _context.Set<Subject>().AnyAsync(e => e.Code == code);
        }
    }
}
