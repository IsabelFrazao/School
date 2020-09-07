using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using Syncfusion.EJ2.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IClassRepository _classRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public StudentsController(IStudentRepository studentRepository, ICourseRepository courseRepository, IClassRepository classRepository, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _classRepository = classRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: StudentsController
        public IActionResult Index()
        {
            return View(_studentRepository.GetAll());
        }

        // GET: StudentsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            student.Course = await _courseRepository.GetByIdAsync(student.CourseId);

            student.Class = await _classRepository.GetByIdAsync(student.ClassId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: StudentsController/Create
        public IActionResult Create()
        {
            ViewBag.idCount = (_studentRepository.GetAll().Count() + 1).ToString();

            var model = new StudentViewModel{Courses = _courseRepository.GetAll().Where(c => c.Id > 1), Classes = _classRepository.GetAll()};

            var school = string.Empty;

            foreach(var year in model.Courses)
            {
                school = $"{ year.BeginDate.Year} / {year.EndDate.Year}";
                model.SchoolYears.Add(school);
            }

            return View(model);
        }

        // POST: StudentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            var path = string.Empty;

            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Students");
                }

                //model.Course = await _courseRepository.GetByIdAsync(model.CourseId);

                //model.Class = await _classRepository.GetByIdAsync(model.ClassId);

                var student = _converterHelper.ToStudent(model, path, await _courseRepository.GetByIdAsync(model.CourseId), await _classRepository.GetByIdAsync(model.ClassId), true);
                
                await _studentRepository.CreateAsync(student);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: StudentsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);

            //student.Course = await _courseRepository.GetByIdAsync(student.CourseId);

            //student.Class = await _classRepository.GetByIdAsync(student.ClassId);

            if (student == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId), _courseRepository.GetAll().Where(c => c.Id > 1), await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll());

            //model.Courses = _courseRepository.GetAll().Where(c => c.Id > 1);

            //model.Classes = _classRepository.GetAll();

            var school = string.Empty;

            foreach (var year in model.Courses)
            {
                school = $"{year.BeginDate.Year} / {year.EndDate.Year}";
                model.SchoolYears.Add(school);
            }

            return View(model);
        }

        // POST: StudentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.PhotoUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Students");
                    }

                    //model.Course = await _courseRepository.GetByIdAsync(model.CourseId);

                    //model.Class = await _classRepository.GetByIdAsync(model.ClassId);

                    var student = _converterHelper.ToStudent(model, path, await _courseRepository.GetByIdAsync(model.CourseId), await _classRepository.GetByIdAsync(model.ClassId), true);

                    student.Id = model.Id;

                    await _studentRepository.UpdateAsync(student);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _studentRepository.ExistsAsync(model.Id))
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

        // GET: StudentsController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            student.Course = await _courseRepository.GetByIdAsync(student.CourseId);

            student.Class = await _classRepository.GetByIdAsync(student.ClassId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: StudentsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            await _studentRepository.DeleteAsync(student);
            return RedirectToAction(nameof(Index));
        }
    }
}
