using School.Web.Data.Entities;

namespace School.Web.Data.Repositories
{
    public class StudentSubjectRepository : GenericRepository<StudentSubject>, IStudentSubjectRepository
    {
        public StudentSubjectRepository(DataContext context) : base(context)
        {

        }
    }
}
