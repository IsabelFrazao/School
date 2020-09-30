using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IClassRepository _classRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public TeachersController(ITeacherRepository teacherRepository, IClassRepository classRepository, ICourseRepository courseRepository,
            ISubjectRepository subjectRepository, IImageHelper imageHelper, IConverterHelper converterHelper, IUserHelper userHelper, IMailHelper mailHelper)
        {
            _teacherRepository = teacherRepository;
            _classRepository = classRepository;
            _courseRepository = courseRepository;
            _subjectRepository = subjectRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        // GET: TeachersController
        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Teacher"))
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                return View(_teacherRepository.GetAll().Where(a => a.isActive == true).Where(u => u.Email == user.Email));
            }
            else
                return View(_teacherRepository.GetAll().Where(c => c.Id > 1).Where(a => a.isActive == true));
        }

        // GET: TeachersController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TeacherNotFound");
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToTeacherViewModel(teacher, _subjectRepository.GetAll().Where(c => c.TeacherId == id.Value).Where(a => a.isActive == true));

            if (model.Subjects != null)
            {
                foreach (var subject in model.Subjects)
                {
                    model.Courses = _courseRepository.GetAll().Where(c => c.Id == subject.CourseId).Where(a => a.isActive == true);
                }
            }

            if (model.Courses != null)
            {
                foreach (var course in model.Courses)
                {
                    model.Classes = _classRepository.GetAll().Where(c => c.CourseId == course.Id).Where(a => a.isActive == true);
                }
            }

            if (teacher == null)
            {
                return new NotFoundViewResult("TeacherNotFound");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: TeachersController/Create
        public IActionResult Create()
        {
            ViewBag.idCount = (_teacherRepository.GetAll().Count() + 1).ToString();

            var model = new TeacherViewModel
            {
                Courses = _courseRepository.GetAll().Where(c => c.Id > 1).Where(a => a.isActive == true),
                Classes = _classRepository.GetAll().Where(a => a.isActive == true),
                Subjects = _subjectRepository.GetAll().Where(a => a.isActive == true)
            };

            return View(model);
        }

        // POST: TeachersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            var path = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ImageFile != null)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Teachers");
                    }

                    var teacher = _converterHelper.ToTeacher(model, path, true);

                    teacher.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    try
                    {
                        await _teacherRepository.CreateAsync(teacher);

                        //USER

                        var user = await _userHelper.GetUserByEmailAsync(teacher.Email);

                        var random = new Random();
                        var password = random.Next(100000, 999999).ToString();

                        if (user == null)
                        {
                            user = new User
                            {
                                FirstName = teacher.FullName,
                                Email = teacher.Email,
                                UserName = teacher.Email
                            };

                            var result = await _userHelper.AddUserAsync(user, password);

                            if (result != IdentityResult.Success)
                            {
                                throw new InvalidOperationException("Could not create the user in seeder");
                            }
                        }

                        var isInRole = await _userHelper.IsUserInRoleAsync(user, "Teacher");

                        if (!isInRole)
                        {
                            await _userHelper.AddUserToRoleAsync(user, "Teacher");
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

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Identification Number, Tax Number, SS Number, NHS Number, Telephone or E-mail already exists!");
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
        // GET: TeachersController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TeacherNotFound");
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);

            if (teacher == null)
            {
                return new NotFoundViewResult("TeacherNotFound");
            }

            var model = _converterHelper.ToTeacherViewModel(teacher, _subjectRepository.GetAll().Where(c => c.TeacherId == id.Value).Where(a => a.isActive == true));

            return View(model);
        }

        // POST: TeachersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.PhotoUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Teachers");
                    }

                    var teacher = _converterHelper.ToTeacher(model, path, true);

                    teacher.Id = model.Id;

                    teacher.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    try
                    {
                        await _teacherRepository.UpdateAsync(teacher);

                        //USER

                        var user = await _userHelper.GetUserByEmailAsync(teacher.Email);

                        if (user == null)
                        {
                            var random = new Random();
                            var password = random.Next(100000, 999999).ToString();

                            user = new User
                            {
                                FirstName = teacher.FullName,
                                Email = teacher.Email,
                                UserName = teacher.Email
                            };

                            var result = await _userHelper.AddUserAsync(user, password);

                            if (result != IdentityResult.Success)
                            {
                                throw new InvalidOperationException("Could not create the user in seeder");
                            }

                            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Teacher");

                            if (!isInRole)
                            {
                                await _userHelper.AddUserToRoleAsync(user, "Teacher");
                            }

                            var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                            var tokenLink = this.Url.Action("ConfirmEmail", "Accounts", new
                            {
                                userId = user.Id,
                                token = myToken,
                            }, protocol: HttpContext.Request.Scheme);

                            _mailHelper.SendMail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                                $"To allow the user, " +
                                $"please click on this link:<br/><br/><a href = \"{tokenLink}\">Confirm Email</a>" +
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
                            ModelState.AddModelError(string.Empty, "Identification Number, Tax Number, SS Number, NHS Number, Telephone or E-mail already exists!");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                        }
                    }                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teacherRepository.ExistsAsync(model.Id))
                    {
                        return new NotFoundViewResult("TeacherNotFound");
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
        // GET: TeachersController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TeacherNotFound");
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);//Value pq pode vir nulo

            var model = _converterHelper.ToTeacherViewModel(teacher, _subjectRepository.GetAll().Where(a => a.isActive == true));

            if (model.Subjects != null)
            {
                foreach (var subject in model.Subjects)
                {
                    model.Courses = _courseRepository.GetAll().Where(c => c.Id == subject.CourseId).Where(a => a.isActive == true);
                }
            }

            if (model.Courses != null)
            {
                foreach (var course in model.Courses)
                {
                    model.Classes = _classRepository.GetAll().Where(c => c.CourseId == course.Id).Where(a => a.isActive == true);
                }
            }

            if (teacher == null)
            {
                return new NotFoundViewResult("TeacherNotFound");
            }

            return View(model);
        }

        // POST: TeachersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);
                await _teacherRepository.DeleteAsync(teacher);
                var user = await _userHelper.GetUserByEmailAsync(teacher.Email);
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
