using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
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

        /// <summary>
        /// Checks the Database is a Subject with the selected Code already exists
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> ExistsCodeAsync(string code)
        {
            return await _context.Set<Subject>().AnyAsync(e => e.Code == code);
        }

        /// <summary>
        /// Returns a Subject by its Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Subject> GetByCodeAsync(string code)
        {
            return await _context.Set<Subject>().FirstOrDefaultAsync(e => e.Code == code);//e=Entity Genérica
        }
    }
}
