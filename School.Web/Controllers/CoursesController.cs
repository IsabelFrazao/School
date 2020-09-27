using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IClassRepository _classRepository;
        private readonly IIEFPSubjectRepository _iefpSubjectRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IConverterHelper _converterHelper;

        public CoursesController(ICourseRepository courseRepository, ITeacherRepository teacherRepository, IClassRepository classRepository,
            IIEFPSubjectRepository iefpSubjectRepository, ISubjectRepository subjectRepository, IConverterHelper converterHelper)
        {
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _classRepository = classRepository;
            _iefpSubjectRepository = iefpSubjectRepository;
            _subjectRepository = subjectRepository;
            _converterHelper = converterHelper;
        }

        // GET: CoursesController
        public IActionResult Index()
        {
            return View(_courseRepository.GetAll().Where(c => c.Id > 1));
        }

        // GET: CoursesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            //course.Coordinator = await _teacherRepository.GetByIdAsync(course.CoordinatorId);

            var model = _converterHelper.ToCourseViewModel(course, await _teacherRepository.GetByIdAsync(course.CoordinatorId),
                 _teacherRepository.GetAll().Where(c => c.Id > 1), _classRepository.GetAll().Where(e => e.CourseId == id.Value),
                 _subjectRepository.GetAll().Where(e => e.CourseId == id.Value));

            //model.Classes = _classRepository.GetAll().Where(e => e.CourseId == id.Value);

            if (course == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: CoursesController/Create
        public IActionResult Create()
        {
            var model = new CourseViewModel
            {
                Teachers = _teacherRepository.GetAll().Where(c => c.Id > 1).ToList(),
                Subjects = _subjectRepository.GetAll()
                .Where(e => e.Field == "Áudiovisuais e Produção dos Media" || e.Field == "Ciências Informáticas" ||
                e.Field == "Eletrónica e Automação").ToList()
            };

            //model.IEFPSubjects = _iefpSubjectRepository.GetAll().Where(e => e.Field == "Áudiovisuais e Produção dos Media" || e.Field == "Ciências Informáticas" || e.Field == "Eletrónica e Automação");//Filter by Field

            return View(model);
        }

        // POST: CoursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Coordinator = await _teacherRepository.GetByIdAsync(model.CoordinatorId);

                var course = _converterHelper.ToCourse(model, true);

                await _courseRepository.CreateAsync(course);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: CoursesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(id.Value);

            //course.Coordinator = await _teacherRepository.GetByIdAsync(course.CoordinatorId);

            if (course == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToCourseViewModel(course, await _teacherRepository.GetByIdAsync(course.CoordinatorId),
                 _teacherRepository.GetAll().Where(c => c.Id > 1), _classRepository.GetAll().Where(e => e.CourseId == id.Value),
                 _subjectRepository.GetAll().Where(e => e.CourseId == id.Value));

            //model.Classes = _classRepository.GetAll().Where(e => e.CourseId == id.Value);

            //model.Teachers = _teacherRepository.GetAll();

            return View(model);
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseRepository.UpdateAsync(course);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _courseRepository.ExistsAsync(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        // GET: CoursesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToCourseViewModel(course, await _teacherRepository.GetByIdAsync(course.CoordinatorId),
                 _teacherRepository.GetAll().Where(c => c.Id > 1), _classRepository.GetAll().Where(e => e.CourseId == id.Value),
                 _subjectRepository.GetAll().Where(e => e.CourseId == id.Value));

            if (course == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: CoursesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            await _courseRepository.DeleteAsync(course);
            return RedirectToAction(nameof(Index));
        }
    }
}
