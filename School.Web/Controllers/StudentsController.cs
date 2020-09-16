using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using Syncfusion.EJ2.Linq;
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
        private readonly IStudentSubjectRepository _studentSubjectRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;

        public StudentsController(IStudentRepository studentRepository, ICourseRepository courseRepository, IClassRepository classRepository,
            ISubjectRepository subjectRepository, IStudentSubjectRepository studentSubjectRepository, ITeacherRepository teacherRepository,
            IGradeRepository gradeRepository, IScheduleRepository scheduleRepository, IImageHelper imageHelper, IConverterHelper converterHelper,
            ICombosHelper combosHelper)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _studentSubjectRepository = studentSubjectRepository;
            _teacherRepository = teacherRepository;
            _gradeRepository = gradeRepository;
            _scheduleRepository = scheduleRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
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

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1), await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll());

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

                //var validate = await _studentRepository.ValidationAsync(student.IdentificationNumber, student.TaxNumber, student.SSNumber,
                //    student.NHSNumber, student.Telephone, student.Email);

                if (!await _studentRepository.ValidationAsync(student.IdentificationNumber, student.TaxNumber, student.SSNumber,
                    student.NHSNumber, student.Telephone, student.Email))
                    await _studentRepository.CreateAsync(student);


                //STUDENTSUBJECT

                var Subjects = _subjectRepository.GetAll().Where(s => s.CourseId == student.CourseId);

                List<StudentSubject> StudentSubjects = new List<StudentSubject>();

                if (Subjects != null)
                {
                    foreach (var subject in Subjects)
                    {
                        if (student.CourseId == subject.CourseId)
                        {
                            StudentSubjects.Add(new StudentSubject
                            {
                                StudentId = student.Id,
                                SubjectId = subject.Id,
                            });
                        }
                    }

                    foreach (var ss in StudentSubjects)
                    {
                        await _studentSubjectRepository.CreateAsync(ss);
                    }

                    var stu = new Student();
                    var sub = new Subject();
                    var course = new Course();
                    var classes = new Class();
                    var teacher = new Teacher();

                    var StudentsSubjects = new List<StudentSubject>(_studentSubjectRepository.GetAll());
                    var Stus = new List<Student>(_studentRepository.GetAll());
                    var Subs = new List<Subject>(_subjectRepository.GetAll());
                    var Courses = new List<Course>(_courseRepository.GetAll());
                    var Classes = new List<Class>(_classRepository.GetAll());
                    var Teachers = new List<Teacher>(_teacherRepository.GetAll());

                    foreach (var ss in StudentsSubjects)
                    {
                        foreach (var s in Stus)
                        {
                            if (s.Id == ss.StudentId)
                            {
                                stu = s;
                            }

                            foreach (var c in Courses)
                            {
                                if (s.CourseId == c.Id)
                                {
                                    course = c;
                                }
                            }

                            foreach (var cl in Classes)
                            {
                                if (s.ClassId == cl.Id)
                                {
                                    classes = cl;
                                }
                            }
                        }

                        foreach (var subj in Subs)
                        {
                            if (sub.Id == ss.SubjectId)
                            {
                                sub = subj;
                            }

                            foreach (var t in Teachers)
                            {
                                if (t.Id == subj.TeacherId)
                                {
                                    teacher = t;
                                }
                            }
                        }

                        var grade = _converterHelper.CreateGrade(ss, stu, sub, course, classes, teacher, true);

                        if (!await _gradeRepository.ExistsAsync(ss.Id))
                        {
                            await _gradeRepository.CreateAsync(grade);
                        }
                    }
                    //if (!validate)
                    //{
                    //    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    //    return Json(new { responseText = "Error" });
                    //}
                    //else
                    //{
                    //    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    //    return Json(new { responseText = "Success" });
                    //}
                }
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

            if (student == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1), await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll());

            model.ComboCourse = _combosHelper.GetComboCourses();

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

                    var student = _converterHelper.ToStudent(model, path, await _courseRepository.GetByIdAsync(model.CourseId),
                        await _classRepository.GetByIdAsync(model.ClassId), true);

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

            var student = await _studentRepository.GetByIdAsync(id.Value);

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1), await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll());

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
            var s = new StudentSubject();
            var g = new Grade();
            foreach (var stu in _studentSubjectRepository.GetAll().Where(t => t.StudentId == student.Id))
            {
                s.StudentId = 0;
                await _studentSubjectRepository.UpdateAsync(s);
            }
            foreach (var stu in _gradeRepository.GetAll().Where(t => t.StudentId == student.Id))
            {
                g.StudentId = 0;
                await _gradeRepository.UpdateAsync(g);
            }
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
