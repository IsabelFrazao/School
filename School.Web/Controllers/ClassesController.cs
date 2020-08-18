using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IClassRepository _classRepository;

        public ClassesController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        // GET: ClassesController
        public IActionResult Index()
        {
            return View(_classRepository.GetAll());
        }

        // GET: ClassesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // GET: ClassesController/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: ClassesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Class classes)
        {
            if (ModelState.IsValid)
            {
                await _classRepository.CreateAsync(classes);
                return RedirectToAction(nameof(Index));
            }
            return View(classes);
        }

        // GET: ClassesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);

            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // POST: ClassesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Class classes)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classRepository.UpdateAsync(classes);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _classRepository.ExistsAsync(classes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(classes);
        }

        // GET: ClassesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // POST: ClassesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classes = await _classRepository.GetByIdAsync(id);
            await _classRepository.DeleteAsync(classes);
            return RedirectToAction(nameof(Index));
        }
    }
}
