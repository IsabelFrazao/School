using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
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

        public CoursesController(ICourseRepository courseRepository, ITeacherRepository teacherRepository, IClassRepository classRepository, IIEFPSubjectRepository iefpSubjectRepository, ISubjectRepository subjectRepository, IConverterHelper converterHelper)
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
            return View(_courseRepository.GetAll());
        }

        // GET: CoursesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var coordinator = await _teacherRepository.GetByIdAsync(course.CoordinatorId);
            course.Coordinator = coordinator;

            var model = _converterHelper.ToCourseViewModel(course);

            var classes = _classRepository.GetAll().Where(e => e.CourseId == id.Value);
            model.Classes = classes;

            if (course == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: CoursesController/Create
        public IActionResult Create()
        {
            var model = new CourseViewModel { Teachers = _teacherRepository.GetAll() };

            model.IEFPSubjects = _iefpSubjectRepository.GetAll().Where(e => e.Field == "Áudiovisuais e Produção dos Media" || e.Field == "Ciências Informáticas" || e.Field == "Eletrónica e Automação");//Filter by Field

            return View(model);
        }


        // POST: CoursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var coordinator = await _teacherRepository.GetByIdAsync(model.CoordinatorId);
                model.Coordinator = coordinator;

                var course = _converterHelper.ToCourse(model, true);

                await _courseRepository.CreateAsync(course);

                foreach (var subject in model.Subjects)
                {
                    subject.Course = model;
                    await _subjectRepository.UpdateAsync(subject);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: CoursesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(id.Value);

            var coordinator = await _teacherRepository.GetByIdAsync(course.CoordinatorId);
            course.Coordinator = coordinator;

            if (course == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToCourseViewModel(course);

            var classes = _classRepository.GetAll().Where(e => e.CourseId == id.Value);
            model.Classes = classes;

            model.Teachers = _teacherRepository.GetAll();

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

        // GET: CoursesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
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
