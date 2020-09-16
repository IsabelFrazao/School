using School.Web.Data.Entities;

namespace School.Web.Data.Repositories
{
    public class ClassroomRepository : GenericRepository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(DataContext context) : base(context)
        {

        }
    }
}
