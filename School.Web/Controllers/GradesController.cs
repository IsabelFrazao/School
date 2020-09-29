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

        public GradesController(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        // GET: GradesController
        public IActionResult Index()
        {
            return View(_gradeRepository.GetAll().Where(a => a.isActive == true)
                .Include(c => c.Course)
                .Include(c => c.Class)
                .Include(c => c.Subject).Where(a => a.isActive == true)
                .Include(c => c.Teacher)
                .Include(c => c.Student).Where(a => a.isActive == true)
                .Where(g => g.FinalGrade >= 0));
        }

        [Authorize(Roles = "Admin")]
        // GET: GradesController/Create
        public IActionResult Create()
        {
            return View(_gradeRepository.GetAll().Where(a => a.isActive == true)
                .Include(c => c.Course)
                .Include(c => c.Class)
                .Include(c => c.Subject).Where(a => a.isActive == true)
                .Include(c => c.Teacher)
                .Include(c => c.Student).Where(a => a.isActive == true));
        }

        // POST: GradesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create([FromBody] GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var grade = await _gradeRepository.GetByIdAsync(model.Id);

                    grade.FinalGrade = model.FinalGrade;
                    grade.Approval = model.FinalGrade >= 10 ? model.Approval = "Approved" : model.Approval = "Disapproved";

                    if (grade.FinalGrade >= 0 && grade.FinalGrade <= 20)
                    {
                        await _gradeRepository.UpdateAsync(grade);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }                
            }
        }
       
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
