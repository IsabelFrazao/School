using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IClassRepository _classRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IConverterHelper _converterHelper;

        public ClassesController(IClassRepository classRepository, ICourseRepository courseRepository, ITeacherRepository teacherRepository,
            IStudentRepository studentRepository, ISubjectRepository subjectRepository, IConverterHelper converterHelper)
        {
            _classRepository = classRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
            _converterHelper = converterHelper;
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

            //classes.Course = await _courseRepository.GetByIdAsync(classes.CourseId);

            var model = _converterHelper.ToClassViewModel(classes, await _courseRepository.GetByIdAsync(classes.CourseId),
                _studentRepository.GetAll().Where(e => e.ClassId == id.Value),
                _subjectRepository.GetAll().Where(e => e.CourseId == classes.CourseId));

            //foreach (var subject in model.Subjects)
            //{
            //   model.Teachers = model.Teachers.All(e => e.Id == subject.TeacherId /*await _teacherRepository.GetByIdAsync(subject.TeacherId)*/);
            //}

            //var students = _studentRepository.GetAll().Where(e => e.ClassId == id.Value);
            //model.Students = students;

            if (classes == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: ClassesController/Create
        public IActionResult Create()
        {
            var model = new ClassViewModel { Courses = _courseRepository.GetAll().Where(c => c.Id > 1), Students = _studentRepository.GetAll() };

            //model.Students = _studentRepository.GetAll().Where(e => e.ClassId == 0);

            return View(model);
        }

        // POST: ClassesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var course = await _courseRepository.GetByIdAsync(model.CourseId);
                //model.Course = course;

                var classes = _converterHelper.ToClass(model, await _courseRepository.GetByIdAsync(model.CourseId), true);

                //foreach(var student in model.Students)
                //{
                //    student.ClassId = model.Id;
                //    await _studentRepository.UpdateAsync(student);
                //}

                await _classRepository.CreateAsync(classes);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ClassesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);

            //classes.Course = await _courseRepository.GetByIdAsync(classes.CourseId);

            if (classes == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToClassViewModel(classes, await _courseRepository.GetByIdAsync(classes.CourseId), _studentRepository.GetAll().Where(e => e.ClassId == id.Value), _subjectRepository.GetAll().Where(e => e.CourseId == classes.CourseId));

            //var students = _studentRepository.GetAll().Where(e => e.ClassId == id.Value);
            //model.Students = students;

            return View(model);
        }

        // POST: ClassesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //model.Course = await _courseRepository.GetByIdAsync(model.CourseId);

                    var classes = _converterHelper.ToClass(model, await _courseRepository.GetByIdAsync(model.CourseId), true);

                    classes.Id = model.Id;

                    await _classRepository.UpdateAsync(classes);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _classRepository.ExistsAsync(model.Id))
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
