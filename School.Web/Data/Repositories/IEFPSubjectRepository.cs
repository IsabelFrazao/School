using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public class IEFPSubjectRepository : GenericRepository<IEFPSubject>, IIEFPSubjectRepository
    {
        private readonly DataContext _context;

        public IEFPSubjectRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsCodeAsync(string code)
        {
            return await _context.Set<IEFPSubject>().AnyAsync(e => e.Code == code);
        }

        public async Task<Subject> GetByCodeAsync(string code)
        {
            return await _context.Set<Subject>().FirstOrDefaultAsync(e => e.Code == code);//e=Entity Genérica
        }
    }
}
