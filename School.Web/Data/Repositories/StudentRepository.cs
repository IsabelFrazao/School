using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly DataContext _context;

        public StudentRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ValidationAsync(string identificationNumber, string taxNumber, string ssNumber, string nhsNumber,
            string telephone, string email)
        {
            if (await _context.Set<Student>().AnyAsync(e => e.IdentificationNumber == identificationNumber))
                return true;

            if (await _context.Set<Student>().AnyAsync(e => e.TaxNumber == taxNumber))
                return true;

            if (await _context.Set<Student>().AnyAsync(e => e.SSNumber == ssNumber))
                return true;

            if (await _context.Set<Student>().AnyAsync(e => e.NHSNumber == nhsNumber))
                return true;

            if (await _context.Set<Student>().AnyAsync(e => e.Telephone == telephone))
                return true;

            if (await _context.Set<Student>().AnyAsync(e => e.Email == email))
                return true;

            return false;
        }
    }
}
