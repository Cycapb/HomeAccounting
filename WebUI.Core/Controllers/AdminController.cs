﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.Core.Infrastructure.Identity.Models;
using WebUI.Core.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        private readonly UserManager<AccountingUserModel> _userManager;
        private readonly IUserValidator<AccountingUserModel> _userValidator;
        private readonly IPasswordValidator<AccountingUserModel> _passwordValidator;
        private readonly IPasswordHasher<AccountingUserModel> _passwordHasher;

        public AdminController(
            UserManager<AccountingUserModel> userManager,
            IUserValidator<AccountingUserModel> userValidator,
            IPasswordValidator<AccountingUserModel> passwordValidator,
            IPasswordHasher<AccountingUserModel> passwordHasher)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return PartialView("_Index", _userManager.Users);
        }

        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AccountingUserModel() { Email = model.Email, UserName = model.Name };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                AddModelErrors(result);
            }
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                var result = await _userManager.DeleteAsync(userToDelete);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            return View("Error", new string[] { "Пользователь не найден" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var userModel = new EditModel()
                {
                    Id = user.Id,
                    Email = user.Email
                };

                return PartialView("_Edit", userModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditModel model)
        {
            var accUser = await _userManager.FindByIdAsync(model.Id);
            if (accUser != null)
            {
                accUser.Email = model.Email;
                var validMail = await _userValidator.ValidateAsync(_userManager, accUser);
                if (!validMail.Succeeded)
                {
                    AddModelErrors(validMail);
                }
                IdentityResult validPassword = null;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    validPassword = await _passwordValidator.ValidateAsync(_userManager, accUser, model.Password);

                    if (!validPassword.Succeeded)
                    {
                        AddModelErrors(validPassword);
                    }
                    else
                    {
                        accUser.PasswordHash = _passwordHasher.HashPassword(accUser, model.Password);
                    }
                }

                if ((validMail.Succeeded && validPassword == null) || (validMail.Succeeded && validPassword.Succeeded && model.Password != string.Empty))
                {
                    var result = await _userManager.UpdateAsync(accUser);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    AddModelErrors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return PartialView("_Edit", model);
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}