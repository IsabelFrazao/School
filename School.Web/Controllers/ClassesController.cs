using Microsoft.AspNetCore.Authorization;
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
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IConverterHelper _converterHelper;

        public ClassesController(IClassRepository classRepository, ICourseRepository courseRepository, ITeacherRepository teacherRepository,
            IStudentRepository studentRepository, ISubjectRepository subjectRepository, IScheduleRepository scheduleRepository,
            IClassroomRepository classroomRepository, IConverterHelper converterHelper)
        {
            _classRepository = classRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
            _scheduleRepository = scheduleRepository;
            _classroomRepository = classroomRepository;
            _converterHelper = converterHelper;
        }

        // GET: ClassesController
        public IActionResult Index()
        {
            return View(_classRepository.GetAll()
                .Include(c => c.Course)
                .Include(c => c.Classroom)
                .Include(c => c.Schedule));
        }

        [Authorize(Roles = "Admin, Teacher")]
        // GET: ClassesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToClassViewModel(classes, await _courseRepository.GetByIdAsync(classes.CourseId),
                _studentRepository.GetAll().Where(e => e.ClassId == id.Value),
                _subjectRepository.GetAll().Where(e => e.CourseId == classes.CourseId));

            model.Teachers = new List<Teacher>();

            IEnumerable<Teacher> Teachers = _teacherRepository.GetAll();

            var Subjects = new List<Subject>(model.Subjects);

            int rep = 0;

            foreach (var subject in Subjects)
            {
                foreach(var teacher in Teachers)
                {
                    if(teacher.Id == subject.TeacherId && teacher.Id != rep)
                    {
                        model.Teachers.Add(new Teacher //FAZER MÉTODO DE CONVERSÃO PARA TEACHER
                        {
                            Id = teacher.Id,
                            PhotoUrl = teacher.PhotoUrl,
                            FullName = teacher.FullName,
                            Gender = teacher.Gender,
                            DateOfBirth = teacher.DateOfBirth,
                            Address = teacher.Address,
                            ZipCode = teacher.ZipCode,
                            City = teacher.City,
                            IdentificationNumber = teacher.IdentificationNumber,
                            TaxNumber = teacher.TaxNumber,
                            SSNumber = teacher.SSNumber,
                            NHSNumber = teacher.NHSNumber,
                            MaritalStatus = teacher.MaritalStatus,
                            Nationality = teacher.Nationality,
                            Telephone = teacher.Telephone,
                            Email = teacher.Email
                        });

                        rep = teacher.Id;
                    }
                }                
            }

            var students = _studentRepository.GetAll().Where(e => e.ClassId == id.Value);
            model.Students = students;

            model.Schedule = await _scheduleRepository.GetByIdAsync(classes.ScheduleId);
            model.Classroom = await _classroomRepository.GetByIdAsync(classes.ClassroomId);

            if (classes == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: ClassesController/Create
        public IActionResult Create()
        {
            var model = new ClassViewModel { Courses = _courseRepository.GetAll().Where(c => c.Id > 1).ToList(), Students = _studentRepository.GetAll().ToList(),
            Classrooms = _classroomRepository.GetAll().ToList(), Schedules = _scheduleRepository.GetAll().ToList()
            };

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

        [Authorize(Roles = "Admin")]
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

            model.Schedules = _scheduleRepository.GetAll();
            model.Classrooms = _classroomRepository.GetAll();

            model.Schedule = await _scheduleRepository.GetByIdAsync(classes.ScheduleId);
            model.Classroom = await _classroomRepository.GetByIdAsync(classes.ClassroomId);
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

        [Authorize(Roles = "Admin")]
        // GET: ClassesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _classRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToClassViewModel(classes, await _courseRepository.GetByIdAsync(classes.CourseId),
                _studentRepository.GetAll().Where(e => e.ClassId == id.Value),
                _subjectRepository.GetAll().Where(e => e.CourseId == classes.CourseId));

            model.Teachers = new List<Teacher>();

            IEnumerable<Teacher> Teachers = _teacherRepository.GetAll();

            var Subjects = new List<Subject>(model.Subjects);

            int rep = 0;

            foreach (var subject in Subjects)
            {
                foreach (var teacher in Teachers)
                {
                    if (teacher.Id == subject.TeacherId && teacher.Id != rep)
                    {
                        model.Teachers.Add(new Teacher
                        {
                            Id = teacher.Id,
                            PhotoUrl = teacher.PhotoUrl,
                            FullName = teacher.FullName,
                            Gender = teacher.Gender,
                            DateOfBirth = teacher.DateOfBirth,
                            Address = teacher.Address,
                            ZipCode = teacher.ZipCode,
                            City = teacher.City,
                            IdentificationNumber = teacher.IdentificationNumber,
                            TaxNumber = teacher.TaxNumber,
                            SSNumber = teacher.SSNumber,
                            NHSNumber = teacher.NHSNumber,
                            MaritalStatus = teacher.MaritalStatus,
                            Nationality = teacher.Nationality,
                            Telephone = teacher.Telephone,
                            Email = teacher.Email
                        });

                        rep = teacher.Id;
                    }
                }
            }

            var students = _studentRepository.GetAll().Where(e => e.ClassId == id.Value);
            model.Students = students;

            model.Schedule = await _scheduleRepository.GetByIdAsync(classes.ScheduleId);
            model.Classroom = await _classroomRepository.GetByIdAsync(classes.ClassroomId);

            if (classes == null)
            {
                return NotFound();
            }

            return View(model);
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
