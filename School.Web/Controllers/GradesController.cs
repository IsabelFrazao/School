using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using Syncfusion.EJ2.Linq;
using System.Linq;
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
            ISubjectRepository subjectRepository, ITeacherRepository teacherRepository, IStudentRepository studentRepository,
            IConverterHelper converterHelper)
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
            return View(_gradeRepository.GetAll()
                .Include(c => c.Course)
                .Include(c => c.Class)
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Include(c => c.Student)
                .Where(g => g.FinalGrade >= 0));
        }

        //// GET: GradesController/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var grade = await _gradeRepository.GetByIdAsync(id.Value);

        //    if (grade == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(grade);
        //}

        [Authorize(Roles = "Admin")]
        // GET: GradesController/Create
        public IActionResult Create()
        {
            return View(_gradeRepository.GetAll().Include(c => c.Course)
                .Include(c => c.Class)
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Include(c => c.Student));
        }

        // POST: GradesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create([FromBody] GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = await _gradeRepository.GetByIdAsync(model.Id);

                grade.FinalGrade = model.FinalGrade;
                grade.Approval = model.FinalGrade >= 10 ? model.Approval = "Approved" : model.Approval = "Disapproved";

                if (grade.FinalGrade >= 0 && grade.FinalGrade <= 20)
                {
                    await _gradeRepository.UpdateAsync(grade);
                }
            }
        }

        //    // GET: GradesController/Edit/5
        //    public async Task<IActionResult> Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var grade = await _gradeRepository.GetByIdAsync(id.Value);

        //        if (grade == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(grade);
        //    }

        //    // POST: GradesController/Edit/5
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(Grade grade)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                await _gradeRepository.UpdateAsync(grade);
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!await _gradeRepository.ExistsAsync(grade.Id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }
        //        return View(grade);
        //    }

        //    // GET: GradesController/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var grade = await _gradeRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

        //        if (grade == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(grade);
        //    }

        //    // POST: GradesController/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        var grade = await _gradeRepository.GetByIdAsync(id);
        //        await _gradeRepository.DeleteAsync(grade);
        //        return RedirectToAction(nameof(Index));
        //    }
    }
}
