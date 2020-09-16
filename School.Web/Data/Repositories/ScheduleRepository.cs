using School.Web.Data.Entities;

namespace School.Web.Data.Repositories
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(DataContext context) : base(context)
        {

        }
    }
}
