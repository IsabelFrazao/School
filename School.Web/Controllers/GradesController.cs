using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
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
            return View();
        }


        // POST: GradesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade)
        {
            if (ModelState.IsValid)
            {
                await _gradeRepository.CreateAsync(grade);
                return RedirectToAction(nameof(Index));
            }
            return View(grade);
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
