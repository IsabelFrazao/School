using School.Web.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        IQueryable GetAllWithUsers();
    }
}
