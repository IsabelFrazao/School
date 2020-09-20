using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System.Linq;

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
    }
}
