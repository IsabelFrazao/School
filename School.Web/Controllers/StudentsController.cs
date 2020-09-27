using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public StudentsController(IStudentRepository studentRepository, ICourseRepository courseRepository, IClassRepository classRepository,
            ISubjectRepository subjectRepository, ITeacherRepository teacherRepository,
            IGradeRepository gradeRepository, IScheduleRepository scheduleRepository, IImageHelper imageHelper, IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _teacherRepository = teacherRepository;
            _gradeRepository = gradeRepository;
            _scheduleRepository = scheduleRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
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

            var student = await _studentRepository.GetByIdAsync(id.Value);

            student.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);// ????

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1), await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll());

            model.Schedule = await _scheduleRepository.GetByIdAsync(student.ScheduleId);

            model.Grades = _gradeRepository.GetAll()
                .Where(g => g.StudentId == model.Id)
                .Include(g => g.Course)
                .Include(g => g.Class)
                .Include(g => g.Subject)
                .Include(g => g.Teacher)
                .Where(g => g.FinalGrade >= 0);

            if (student == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: StudentsController/Create
        public IActionResult Create()
        {
            ViewBag.idCount = (_studentRepository.GetAll().Count() + 1).ToString();

            var model = new StudentViewModel { Courses = _courseRepository.GetAll().Where(c => c.Id > 1).ToList(), Classes = _classRepository.GetAll().ToList(),
            Schedules = _scheduleRepository.GetAll().ToList()
            };

            model.SchoolYears = new List<string>();

            foreach(var course in model.Courses)
            {
                model.SchoolYears.Add(course.SchoolYear);
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

                var student = _converterHelper.ToStudent(model, path, await _courseRepository.GetByIdAsync(model.CourseId),
                    await _classRepository.GetByIdAsync(model.ClassId), true);

                student.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (!await _studentRepository.ValidationAsync(student.IdentificationNumber, student.TaxNumber, student.SSNumber,
                    student.NHSNumber, student.Telephone, student.Email))
                    await _studentRepository.CreateAsync(student);

                //USER

                var user = await _userHelper.GetUserByEmailAsync(student.Email);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = student.FullName,
                        Email = student.Email,
                        UserName = student.Email,
                        EmailConfirmed = true
                    };

                    var result = await _userHelper.AddUserAsync(user, "123456");

                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create the user in seeder");
                    }
                }

                var isInRole = await _userHelper.IsUserInRoleAsync(user, "Student");

                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Student");
                }

                //GRADES

                var Subjects = _subjectRepository.GetAll().Where(s => s.CourseId == student.CourseId);

                var Grades = new List<Grade>();

                if (Subjects != null)
                {
                    foreach (var subject in Subjects)
                    {
                        if (student.CourseId == subject.CourseId)
                        {
                            Grades.Add(new Grade {
                                Id = 0,
                                CourseId = student.CourseId,
                                ClassId = student.ClassId,
                                SubjectId = subject.Id,
                                TeacherId = subject.TeacherId,
                                StudentId = student.Id,
                                FinalGrade = -1,
                            });                            
                        }
                    }

                    foreach (var grade in Grades)
                    {
                        await _gradeRepository.CreateAsync(grade);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: StudentsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1), await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll());

            model.Schedules = _scheduleRepository.GetAll();

            model.SchoolYears = new List<string>();

            foreach (var course in model.Courses)
            {
                model.SchoolYears.Add(course.SchoolYear);
            }

            model.Schedule = await _scheduleRepository.GetByIdAsync(student.ScheduleId);

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

                    var student = _converterHelper.ToStudent(model, path, await _courseRepository.GetByIdAsync(model.CourseId),
                        await _classRepository.GetByIdAsync(model.ClassId), true);

                    student.Id = model.Id;

                    student.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

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

        [Authorize(Roles = "Admin")]
        // GET: StudentsController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1), await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll());

            model.Schedule = await _scheduleRepository.GetByIdAsync(student.ScheduleId);

            model.Grades = _gradeRepository.GetAll()
                .Where(g => g.StudentId == model.Id)
                .Include(g => g.Course)
                .Include(g => g.Class)
                .Include(g => g.Subject)
                .Include(g => g.Teacher)
                .Where(g => g.FinalGrade >= 0);

            if (student == null)
            {
                return NotFound();
            }

            return View(model);
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

        public async Task<bool> Validate([FromBody]Student student)
        {
            return await _studentRepository.ValidationAsync(student.IdentificationNumber, student.TaxNumber, student.SSNumber,
                    student.NHSNumber, student.Telephone, student.Email);
        }
    }
}
