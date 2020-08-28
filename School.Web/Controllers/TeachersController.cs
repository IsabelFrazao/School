using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IClassRepository _classRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public TeachersController(ITeacherRepository teacherRepository, IClassRepository classRepository, ICourseRepository courseRepository, ISubjectRepository subjectRepository, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _teacherRepository = teacherRepository;
            _classRepository = classRepository;
            _courseRepository = courseRepository;
            _subjectRepository = subjectRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: TeachersController
        public IActionResult Index()
        {
            return View(_teacherRepository.GetAll());
        }

        // GET: TeachersController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToTeacherViewModel(teacher);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: TeachersController/Create
        public IActionResult Create()
        {
            ViewBag.idCount = (_teacherRepository.GetAll().Count() + 1).ToString();

            var model = new TeacherViewModel { Courses = _courseRepository.GetAll(), Classes = _classRepository.GetAll(), Subjects = _subjectRepository.GetAll() };

            return View(model);
        }


        // POST: TeachersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            var path = string.Empty;

            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Teachers");
                }

                var teacher = _converterHelper.ToTeacher(model, path, true);

                await _teacherRepository.CreateAsync(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: TeachersController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);

            if (teacher == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToTeacherViewModel(teacher);

            return View(model);
        }

        // POST: TeachersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.PhotoUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Teachers");
                    }

                    var teacher = _converterHelper.ToTeacher(model, path, true);

                    teacher.Id = model.Id;

                    await _teacherRepository.UpdateAsync(teacher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teacherRepository.ExistsAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        // GET: TeachersController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: TeachersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            await _teacherRepository.DeleteAsync(teacher);
            return RedirectToAction(nameof(Index));
        }
    }
}
