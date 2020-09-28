using School.Web.Data.Entities;

namespace School.Web.Data.Repositories
{
    public class FieldRepository : GenericRepository<Field>, IFieldRepository
    {
        public FieldRepository(DataContext context) : base(context)
        {

        }
    }
}
