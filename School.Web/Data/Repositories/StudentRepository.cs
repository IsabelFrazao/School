using School.Web.Data.Entities;

namespace School.Web.Data.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(DataContext context) : base(context)
        {

        }
    }
}
