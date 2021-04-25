using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Infrastructure.Identity.Models;
using WebUI.Core.Models;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class UserAccountController : Controller
    {
        private readonly UserManager<AccountingUserModel> _userManager;
        private readonly SignInManager<AccountingUserModel> _signInManager;
        private readonly IPasswordValidator<AccountingUserModel> _passwordValidator;
        private readonly IUserValidator<AccountingUserModel> _userValidator;
        private readonly ILogger _logger = Log.Logger.ForContext<UserAccountController>();

        public UserAccountController(
            UserManager<AccountingUserModel> userManager,
            SignInManager<AccountingUserModel> signInManager,
            IPasswordValidator<AccountingUserModel> passwordValidator,
            IUserValidator<AccountingUserModel> userValidator
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordValidator = passwordValidator;
            _userValidator = userValidator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> LoginDemo()
        {
            var loginModel = new LoginModel()
            {
                Name = "demo",
                Password = "12qw34er"
            };

            return await Login(loginModel, Url.Action("Index", "PayingItem"));
        }

        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return PartialView("_Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(model.Name);

                    if (user != null)
                    {
                        var result = await SignInUser(user, model.Password);

                        if (result.Succeeded)
                        {
                            return Json(new { url = returnUrl, hasErrors = "false" });
                        }
                    }

                    ModelState.AddModelError("", "Неверные имя пользователя или пароль");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Ошибка во время аутентификации");
                    ModelState.AddModelError("", "Сервис временно недоступен. Ведутся работы над восстановлением.");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_Login", model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "PayingItem");
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUserAsync();
                var pass = _userManager.PasswordHasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, model.CurrentPassword);

                if (pass == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("", "Неверный текущий пароль");

                    return PartialView("_ChangePassword", model);
                }

                if (pass == PasswordVerificationResult.Success)
                {
                    var resultPass = await _passwordValidator.ValidateAsync(_userManager, currentUser, model.NewPassword);

                    if (!resultPass.Succeeded)
                    {
                        AddModelErrors(resultPass);
                    }
                    else
                    {
                        currentUser.PasswordHash = _userManager.PasswordHasher.HashPassword(currentUser, model.NewPassword);
                        var updateResult = await _userManager.UpdateAsync(currentUser);

                        if (!updateResult.Succeeded)
                        {
                            AddModelErrors(updateResult);
                        }
                        else
                        {
                            return RedirectToAction("ViewCredentials");
                        }
                    }
                }
            }
            else
            {
                return PartialView("_ChangePassword", model);
            }

            return PartialView("_ChangePassword", model);
        }

        [Authorize]
        public async Task<ActionResult> ViewCredentials()
        {
            var currentIUser = await GetCurrentUserAsync();
            var credentialsModel = new CredentialsModel()
            {
                Email = currentIUser.Email,
                FirstName = currentIUser.FirstName,
                LastName = currentIUser.LastName,
                Login = currentIUser.UserName
            };

            return PartialView("_ViewCredentials", credentialsModel);
        }

        [Authorize]
        public async Task<IActionResult> ChangeCredentials()
        {
            var currentUser = await GetCurrentUserAsync();

            return PartialView("_ChangeCredentials", currentUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangeCredentials(string id, string email, string firstName, string lastName)
        {
            var userToChange = await _userManager.FindByIdAsync(id);

            if (userToChange != null)
            {
                userToChange.Email = email;
                var validEmail = await _userValidator.ValidateAsync(_userManager, userToChange);

                if (!validEmail.Succeeded)
                {
                    AddModelErrors(validEmail);
                }
                else
                {
                    userToChange.FirstName = firstName;
                    userToChange.LastName = lastName;
                    var result = await _userManager.UpdateAsync(userToChange);

                    if (!result.Succeeded)
                    {
                        AddModelErrors(result);
                    }
                    else
                    {
                        return RedirectToAction("ViewCredentials");
                    }
                }
            }
            else
            {
                return View("Error", new string[] { "Ошибка при изменении данных" });
            }

            return PartialView("_ChangeCredentials", userToChange);
        }

        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return PartialView("_Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userToAdd = new AccountingUserModel()
                {
                    UserName = model.Login,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(userToAdd, model.Password);

                if (!result.Succeeded)
                {
                    AddModelErrors(result);

                    return PartialView("_Register", model);
                }

                result = await _userManager.AddToRoleAsync(userToAdd, "Users");

                if (!result.Succeeded)
                {
                    AddModelErrors(result);

                    return PartialView("_Register", model);
                }
                else
                {
                    var signInResult = await SignInUser(userToAdd, model.Password);

                    if (signInResult.Succeeded)
                    {
                        var urlToRedirect = Url.Action("Index", "PayingItem");

                        return Json(new { url = urlToRedirect, hasErrors = "false" });
                    }

                    ModelState.AddModelError("", "Невозможно аутентифицироваться после регистрации");
                }
            }

            return PartialView("_Register", model);
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task<AccountingUserModel> GetCurrentUserAsync()
        {
            return await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        private async Task<Microsoft.AspNetCore.Identity.SignInResult> SignInUser(AccountingUserModel userModel, string password)
        {
            await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(userModel, password, true, false);

            if (result.Succeeded)
            {
                var webUser = new WebUser() { Email = userModel.Email, Name = userModel.UserName, Id = userModel.Id };
                await HttpContext.Session.SetJsonAsync<WebUser>(nameof(WebUser), webUser);
            }

            return result;
        }
    }
}