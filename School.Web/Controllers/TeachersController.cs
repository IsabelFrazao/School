using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeachersController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
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

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: TeachersController/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: TeachersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                await _teacherRepository.CreateAsync(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
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

            return View(teacher);
        }

        // POST: TeachersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _teacherRepository.UpdateAsync(teacher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teacherRepository.ExistsAsync(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(teacher);
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
