using School.Web.Data.Entities;

namespace School.Web.Data.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DataContext context) : base(context)
        {

        }
    }
}
