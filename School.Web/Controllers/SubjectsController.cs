using Microsoft.AspNetCore.Mvc;
using School.Web.Data.Repositories;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectsController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        // GET: SubjectsController
        public IActionResult Index()
        {
            return View(_subjectRepository.GetAll());
        }

        // GET: SubjectsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }
    }
}
