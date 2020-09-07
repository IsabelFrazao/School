using School.Web.Data.Entities;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public interface IIEFPSubjectRepository : IGenericRepository<IEFPSubject>
    {
        Task<bool> ExistsCodeAsync(string code);

        Task<Subject> GetByCodeAsync(string code);
    }
}
