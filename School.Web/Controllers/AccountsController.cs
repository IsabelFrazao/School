﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using School.Web.Data.Entities;
using School.Web.Data.Repositories;
using School.Web.Helpers;
using School.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IFieldRepository _fieldRepository;
        private readonly IMailHelper _mailHelper;

        public AccountsController(IUserHelper userHelper, IConfiguration configuration, IScheduleRepository scheduleRepository,
            IClassroomRepository classroomRepository, IFieldRepository fieldRepository, IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _scheduleRepository = scheduleRepository;
            _classroomRepository = classroomRepository;
            _fieldRepository = fieldRepository;
            _mailHelper = mailHelper;
        }

        /// <summary>
        /// Returns the Login View
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// Saves the Login data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userHelper.LoginAsync(model);

                    if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First());
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            ModelState.AddModelError(string.Empty, "Failed to Login.");
            return View(model);
        }

        /// <summary>
        /// Returns the Logout View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogOutAsync();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns the Register New User View
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Saves the Register New User data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByEmailAsync(model.Username);

                    if (user == null)
                    {
                        user = new User
                        {
                            FirstName = model.FirstName,
                            //LastName = model.LastName,
                            UserName = model.Username
                        };

                        var result = await _userHelper.AddUserAsync(user, model.Password);

                        if (result != IdentityResult.Success)
                        {
                            ModelState.AddModelError(string.Empty, "The user couldn´t be created");
                            return View(model);
                        }

                        var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        var tokenLink = this.Url.Action("ConfirmEmail", "Accounts", new
                        {
                            userId = user.Id,
                            token = myToken,
                        }, protocol: HttpContext.Request.Scheme);

                        _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                            $"To allow the user, " +
                            $"please click on this link:<br/><br/><a href = \"{tokenLink}\">Confirm Email</a>");
                        this.ViewBag.Message = "The instructions to allow your user has been sent to email.";

                        return View(model);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                this.ModelState.AddModelError(string.Empty, "The user already exists.");
            }
            return View(model);
        }

        /// <summary>
        /// Returns the View after E-mail confirmation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns>View</returns>
        public async Task<IActionResult> ConfirmEmail(string userId, string token, string password)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel { OldPassword = password, UserId = userId };

            return View(model);
        }

        /// <summary>
        /// Returns the Change User View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return this.View(model);
        }

        /// <summary>
        /// Change Properties of the User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>View</returns>
        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;

                        var response = await _userHelper.UpdateUserAsync(user);
                        if (response.Succeeded)
                        {
                            ViewBag.UserMessage = "User updated!";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return this.View(model);
        }

        /// <summary>
        /// Returns the Change Password View
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Saves the Change Password data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByIdAsync(model.UserId);

                    if (user == null)
                    {
                        user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    }

                    if (user != null)
                    {
                        var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            if (Request.Query.Keys.Contains("ReturnUrl"))
                            {
                                return Redirect(Request.Query["ReturnUrl"].First());
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                        }
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, "User Not Found!");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View(model);
        }

        /// <summary>
        /// Returns the Recover Password View
        /// </summary>
        /// <returns></returns>
        public IActionResult RecoverPassword()
        {
            return View();
        }

        /// <summary>
        /// Saves the Recover Password data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "The email doesn't correspond to a registered user.");
                        return this.View(model);
                    }

                    var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                    var link = this.Url.Action(
                        "ResetPassword",
                        "Accounts",
                        new { token = myToken }, protocol: HttpContext.Request.Scheme);

                    _mailHelper.SendMail(model.Email, "School Password Reset", $"<h1>School Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                    return this.View();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return this.View(model);
        }

        /// <summary>
        /// Returns the Reset Password View
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        /// <summary>
        /// Saves the Reset Password data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);
                if (user != null)
                {
                    var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        this.ViewBag.Message = "Password reset successful.";
                        return this.View();
                    }

                    this.ViewBag.Message = "Error while resetting the password.";
                    return View(model);
                }

                this.ViewBag.Message = "User not found.";
                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the Not Authorized View
        /// </summary>
        /// <returns></returns>
        public IActionResult NotAuthorized()
        {
            return View();
        }

        /// <summary>
        /// Returns the Settings View
        /// </summary>
        /// <returns></returns>
        public IActionResult Settings()
        {
            var model = new SettingsViewModel
            {
                Schedules = _scheduleRepository.GetAll().Where(a => a.isActive == true),
                Classrooms = _classroomRepository.GetAll().Where(a => a.isActive == true),
                Fields = _fieldRepository.GetAll().Where(a => a.isActive == true)
            };

            return View(model);
        }

        /// <summary>
        /// Gets the data from the Front-End Ajax and adds or updates an item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddSchedule([FromBody] Schedule model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _scheduleRepository.GetByIdAsync(model.Id) == null)
                    {
                        var schedule = new Schedule
                        {
                            Id = 0,
                            Shift = model.Shift,
                            isActive = true
                        };

                        await _scheduleRepository.CreateAsync(schedule);
                    }
                    else
                    {
                        var schedule = await _scheduleRepository.GetByIdAsync(model.Id);

                        schedule.Shift = model.Shift;
                        schedule.isActive = true;

                        await _scheduleRepository.UpdateAsync(schedule);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the data from the Front-End Ajax and adds or updates an item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddClassroom([FromBody] Classroom model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _classroomRepository.GetByIdAsync(model.Id) == null)
                    {
                        var classroom = new Classroom
                        {
                            Id = 0,
                            Room = model.Room,
                            isActive = true
                        };

                        await _classroomRepository.CreateAsync(classroom);
                    }
                    else
                    {
                        var classroom = await _classroomRepository.GetByIdAsync(model.Id);

                        classroom.Room = model.Room;
                        classroom.isActive = true;

                        await _classroomRepository.UpdateAsync(classroom);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the data from the Front-End Ajax and adds or updates an item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddField([FromBody] Field model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _fieldRepository.GetByIdAsync(model.Id) == null)
                    {
                        var field = new Field
                        {
                            Id = 0,
                            Area = model.Area,
                            isActive = true
                        };

                        await _fieldRepository.CreateAsync(field);
                    }
                    else
                    {
                        var field = await _fieldRepository.GetByIdAsync(model.Id);

                        field.Area = model.Area;
                        field.isActive = true;

                        await _fieldRepository.UpdateAsync(field);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the data from the Front-End Ajax and deletes an item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteSchedule([FromBody] Schedule model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _scheduleRepository.GetByIdAsync(model.Id) != null)
                    {
                        var schedule = await _scheduleRepository.GetByIdAsync(model.Id);

                        await _scheduleRepository.DeleteAsync(schedule);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the data from the Front-End Ajax and deletes an item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteClassroom([FromBody] Classroom model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _classroomRepository.GetByIdAsync(model.Id) != null)
                    {
                        var classroom = await _classroomRepository.GetByIdAsync(model.Id);

                        await _classroomRepository.DeleteAsync(classroom);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the data from the Front-End Ajax and deletes an item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteField([FromBody] Field model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _fieldRepository.GetByIdAsync(model.Id) != null)
                    {
                        var field = await _fieldRepository.GetByIdAsync(model.Id);

                        await _fieldRepository.DeleteAsync(field);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }
    }
}
