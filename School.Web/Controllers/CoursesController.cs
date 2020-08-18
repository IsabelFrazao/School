using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepository;

        public CoursesController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
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

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: CoursesController/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: CoursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                await _courseRepository.CreateAsync(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: CoursesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepository.GetByIdAsync(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course course)
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
