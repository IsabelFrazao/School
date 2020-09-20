using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Web.Data.Entities;
using School.Web.Helpers;
using School.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountsController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
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
            ModelState.AddModelError(string.Empty, "Failed to Login.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Username
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn´t be created");
                        return View(model);
                    }

                    //var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    //var tokenLink = this.Url.Action("ConfirmEmail", "Accounts", new
                    //{
                    //    userId = user.Id,
                    //    token = myToken,
                    //}, protocol: HttpContext.Request.Scheme);

                    //_mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    //    $"To allow the user, " +
                    //    $"please click on this link:<br/><br/><a href = \"{tokenLink}\">Confirm Email</a>");
                    //this.ViewBag.Message = "The instructions to allow your user has been sent to email.";

                    /*var loginViewModel = new LoginViewModel
                    {
                        Username = model.Username,
                        Password = model.Password,
                        RememberMe = false,
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                    this.ModelState.AddModelError(string.Empty, "The user couldn´t login");*/

                    ModelState.AddModelError(string.Empty, "The user already exists");
                    return View(model);
                }
            }
            return View(model);
        }

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

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var respose = await _userHelper.UpdateUserAsync(user);
                    if (respose.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return this.View(model);
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
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

            return View(model);
        }
    }
}
