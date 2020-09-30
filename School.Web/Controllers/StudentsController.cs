using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMailHelper _mailHelper;

        public StudentsController(IStudentRepository studentRepository, ICourseRepository courseRepository, IClassRepository classRepository,
            ISubjectRepository subjectRepository, ITeacherRepository teacherRepository,
            IGradeRepository gradeRepository, IScheduleRepository scheduleRepository, IImageHelper imageHelper, IConverterHelper converterHelper,
            IUserHelper userHelper, IMailHelper mailHelper)
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
            _mailHelper = mailHelper;
        }

        // GET: StudentsController
        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Student"))
            { 
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                return View(_studentRepository.GetAll().Where(a => a.isActive == true).Where(u => u.Email == user.Email)); 
            }
            else
                return View(_studentRepository.GetAll().Where(a => a.isActive == true));
        }

        // GET: StudentsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("StudentNotFound");
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);

            student.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);// ????

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1).Where(a => a.isActive == true),
                await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll().Where(a => a.isActive == true));

            model.Schedule = await _scheduleRepository.GetByIdAsync(student.ScheduleId);

            model.Grades = _gradeRepository.GetAll().Where(a => a.isActive == true)
                .Where(g => g.StudentId == model.Id)
                .Include(g => g.Course)
                .Include(g => g.Class)
                .Include(g => g.Subject).Where(a => a.isActive == true)
                .Include(g => g.Teacher)
                .Where(g => g.FinalGrade >= 0);

            if (student == null)
            {
                return new NotFoundViewResult("StudentNotFound");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: StudentsController/Create
        public IActionResult Create()
        {
            ViewBag.idCount = (_studentRepository.GetAll().Count() + 1).ToString();

            var model = new StudentViewModel
            {
                Courses = _courseRepository.GetAll().Where(c => c.Id > 1).Where(a => a.isActive == true).ToList(),
                Classes = _classRepository.GetAll().Where(a => a.isActive == true).ToList(),
                Schedules = _scheduleRepository.GetAll().Where(a => a.isActive == true).ToList()
            };

            model.SchoolYears = new List<string>();

            foreach (var course in model.Courses)
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
                try
                {
                    if (model.ImageFile != null)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Students");
                    }

                    var student = _converterHelper.ToStudent(model, path, await _courseRepository.GetByIdAsync(model.CourseId),
                        await _classRepository.GetByIdAsync(model.ClassId), true);

                    student.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    try
                    {
                        await _studentRepository.CreateAsync(student);

                        //USER

                        var user = await _userHelper.GetUserByEmailAsync(student.Email);

                        var random = new Random();
                        var password = random.Next(100000, 999999).ToString();

                        if (user == null)
                        {
                            user = new User
                            {
                                FirstName = student.FullName,
                                Email = student.Email,
                                UserName = student.Email
                            };

                            var result = await _userHelper.AddUserAsync(user, password);

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

                        var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        var tokenLink = this.Url.Action("ConfirmEmail", "Accounts", new
                        {
                            userId = user.Id,
                            token = myToken,
                        }, protocol: HttpContext.Request.Scheme);

                        _mailHelper.SendMail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                            $"To allow the user, " +
                            $"please click on this link:<br/><br/><a href=\"{tokenLink}\">Confirm Email</a>" +
                            $"<br/><br/> Please, change your Password! <br /><br />Your old password is:<br/><br/><b>{password}</b>.");
                        this.ViewBag.Message = "The instructions to allow your user has been sent to email.";

                        this.ModelState.AddModelError(string.Empty, "The user already exists.");

                        //GRADES

                        var Subjects = _subjectRepository.GetAll().Where(s => s.CourseId == student.CourseId).Where(a => a.isActive == true);

                        var Grades = new List<Grade>();

                        if (Subjects != null)
                        {
                            foreach (var subject in Subjects)
                            {
                                if (student.CourseId == subject.CourseId)
                                {
                                    Grades.Add(new Grade
                                    {
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
                    catch(Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Identification Number, Tax Number, SS Number, NHS Number, Telephone or E-mail already exist!");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }   
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }         
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: StudentsController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("StudentNotFound");
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);

            if (student == null)
            {
                return new NotFoundViewResult("StudentNotFound");
            }

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1).Where(a => a.isActive == true),
                await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll().Where(a => a.isActive == true));

            model.Schedules = _scheduleRepository.GetAll().Where(a => a.isActive == true);

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

                    try
                    {
                        await _studentRepository.UpdateAsync(student);

                        //USER

                        var user = await _userHelper.GetUserByEmailAsync(student.Email);

                        if (user == null)
                        {
                            var random = new Random();
                            var password = random.Next(100000, 999999).ToString();

                            user = new User
                            {
                                FirstName = student.FullName,
                                Email = student.Email,
                                UserName = student.Email
                            };

                            var result = await _userHelper.AddUserAsync(user, password);

                            if (result != IdentityResult.Success)
                            {
                                throw new InvalidOperationException("Could not create the user in seeder");
                            }

                            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Student");

                            if (!isInRole)
                            {
                                await _userHelper.AddUserToRoleAsync(user, "Student");
                            }

                            var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                            var tokenLink = this.Url.Action("ConfirmEmail", "Accounts", new
                            {
                                userId = user.Id,
                                token = myToken,
                            }, protocol: HttpContext.Request.Scheme);

                            _mailHelper.SendMail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                                $"To allow the user, " +
                                $"please click on this link:<br/><br/><a href=\"{tokenLink}\">Confirm Email</a>" +
                                $"<br/><br/> Please, change your Password! <br /><br />Your old password is:<br/><br/><b>{password}</b>.");
                            this.ViewBag.Message = "The instructions to allow your user has been sent to email.";

                            this.ModelState.AddModelError(string.Empty, "The user already exists.");
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Identification Number, Tax Number, SS Number, NHS Number, Telephone or E-mail already exist!");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _studentRepository.ExistsAsync(model.Id))
                    {
                        return new NotFoundViewResult("StudentNotFound");
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
                return new NotFoundViewResult("StudentNotFound");
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);

            var model = _converterHelper.ToStudentViewModel(student, await _courseRepository.GetByIdAsync(student.CourseId),
                _courseRepository.GetAll().Where(c => c.Id > 1).Where(a => a.isActive == true),
                await _classRepository.GetByIdAsync(student.ClassId), _classRepository.GetAll().Where(a => a.isActive == true));

            model.Schedule = await _scheduleRepository.GetByIdAsync(student.ScheduleId);

            model.Grades = _gradeRepository.GetAll().Where(a => a.isActive == true)
                .Where(g => g.StudentId == model.Id)
                .Include(g => g.Course)
                .Include(g => g.Class)
                .Include(g => g.Subject).Where(a => a.isActive == true)
                .Include(g => g.Teacher)
                .Where(g => g.FinalGrade >= 0);

            if (student == null)
            {
                return new NotFoundViewResult("StudentNotFound");
            }

            return View(model);
        }

        // POST: StudentsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);
                await _studentRepository.DeleteAsync(student);
                var user = await _userHelper.GetUserByEmailAsync(student.Email);
                user.isActive = false;
                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }            
        }
    }
}
