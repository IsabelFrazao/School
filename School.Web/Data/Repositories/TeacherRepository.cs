using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly DataContext _context;

        public TeacherRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Teachers.Include(p => p.User);
        }

        public async Task<bool> ValidationAsync(string identificationNumber, string taxNumber, string ssNumber, string nhsNumber,
           string telephone, string email)
        {
            if ((await _context.Set<Student>().AnyAsync(e => e.IdentificationNumber == identificationNumber)) || (await _context.Set<Teacher>().AnyAsync(e => e.IdentificationNumber == identificationNumber)))
                return true;

            if ((await _context.Set<Student>().AnyAsync(e => e.TaxNumber == taxNumber)) || (await _context.Set<Teacher>().AnyAsync(e => e.TaxNumber == taxNumber)))
                return true;

            if ((await _context.Set<Student>().AnyAsync(e => e.SSNumber == ssNumber)) || (await _context.Set<Teacher>().AnyAsync(e => e.SSNumber == ssNumber)))
                return true;

            if ((await _context.Set<Student>().AnyAsync(e => e.NHSNumber == nhsNumber)) || (await _context.Set<Teacher>().AnyAsync(e => e.NHSNumber == nhsNumber)))
                return true;

            if ((await _context.Set<Student>().AnyAsync(e => e.Telephone == telephone)) || (await _context.Set<Teacher>().AnyAsync(e => e.Telephone == telephone)))
                return true;

            if ((await _context.Set<Student>().AnyAsync(e => e.Email == email)) || (await _context.Set<Teacher>().AnyAsync(e => e.Email == email)))
                return true;

            return false;
        }

    }
}
