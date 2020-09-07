using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class GradesController : Controller
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IConverterHelper _converterHelper;

        public GradesController(IGradeRepository gradeRepository, ICourseRepository courseRepository, IClassRepository classRepository,
            ISubjectRepository subjectRepository, ITeacherRepository teacherRepository, IStudentRepository studentRepository, IConverterHelper converterHelper)
        {
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _converterHelper = converterHelper;
        }

        // GET: GradesController
        public IActionResult Index()
        {
            return View(_gradeRepository.GetAll());
        }

        // GET: GradesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _gradeRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // GET: GradesController/Create
        public IActionResult Create()
        {
            var model = new GradeViewModel
            {
                Courses = _courseRepository.GetAll(),
                Classes = _classRepository.GetAll(),
                Subjects = _subjectRepository.GetAll(),
                Teachers = _teacherRepository.GetAll(),
                Students = _studentRepository.GetAll(),
                Grades = _gradeRepository.GetAll()
            };

            return View(model);
        }


        // POST: GradesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = _converterHelper.ToGrade(model, await _courseRepository.GetByIdAsync(model.CourseId), await _classRepository.GetByIdAsync(model.ClassId),
                    await _subjectRepository.GetByIdAsync(model.SubjectId), await _teacherRepository.GetByIdAsync(model.TeacherId),
                    await _studentRepository.GetByIdAsync(model.StudentId), true);

                await _gradeRepository.CreateAsync(grade);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: GradesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _gradeRepository.GetByIdAsync(id.Value);

            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: GradesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Grade grade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _gradeRepository.UpdateAsync(grade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gradeRepository.ExistsAsync(grade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(grade);
        }

        // GET: GradesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _gradeRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: GradesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _gradeRepository.GetByIdAsync(id);
            await _gradeRepository.DeleteAsync(grade);
            return RedirectToAction(nameof(Index));
        }
    }
}
