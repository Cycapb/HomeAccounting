using Loggers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Infrastructure;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize]
    public class UserAccountController : Controller
    {
        private AccUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<AccUserManager>();
        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;
        private AccUserModel CurrentUser => UserManager.FindById(HttpContext.User.Identity.GetUserId());
        private readonly IUserLoginActivityLogger _userReporter;
        private readonly IPlanningHelper _planingHelper;
        private readonly IExceptionLogger _exceptionLogger;

        public UserAccountController(
            IUserLoginActivityLogger userReporter,
            IPlanningHelper planingHelper,
            IExceptionLogger exceptionLogger)
        {
            _userReporter = userReporter;
            _planingHelper = planingHelper;
            _exceptionLogger = exceptionLogger;
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
        public ActionResult Index(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Доступ запрещен" });
            }
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
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
                    var user = await UserManager.FindAsync(model.Name, model.Password);

                    if (user != null)
                    {
                        await SignInUser(user);

                        if (user.UserName == "demo")
                        {
                            var address = HttpContext.Request.UserHostAddress;
                            await _userReporter.Log(user, address);
                        }

                        return Json(new { url = returnUrl, hasErrors = "false" }, JsonRequestBehavior.AllowGet);
                    }

                    ModelState.AddModelError("", "Неверные имя пользователя или пароль");
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogException(ex);
                    ModelState.AddModelError("", "Сервис временно недоступен. Ведутся работы над восстановлением.");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_Login", model);
        }

        private async Task SignInUser(AccUserModel user)
        {
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthManager.SignOut();
            AuthManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

            Session["WebUser"] = new WebUser() { Id = user.Id, Name = user.UserName, Email = user.Email };
        }

        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            Session.Clear();
            OutputCacheAttribute.ChildActionCache = new MemoryCache("NewOutputCache");
            return RedirectToAction("Index", "PayingItem");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var pass = UserManager.PasswordHasher.VerifyHashedPassword(CurrentUser.PasswordHash,
                    model.CurrentPassword);
                if (pass == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("", "Неверный текущий пароль");
                    return PartialView("_ChangePassword", model);
                }
                if (pass == PasswordVerificationResult.Success)
                {
                    if (!model.NewPassword.Equals(model.ConfirmPassword))
                    {
                        ModelState.AddModelError("", "Введенные пароли не сопадают");
                        return PartialView("_ChangePassword", model);
                    }
                    else
                    {
                        var resultPass = await UserManager.PasswordValidator.ValidateAsync(model.NewPassword);
                        if (!resultPass.Succeeded)
                        {
                            AddModelErrors(resultPass);
                        }
                        else
                        {
                            CurrentUser.PasswordHash = UserManager.PasswordHasher.HashPassword(model.NewPassword);
                            var updateResult = await UserManager.UpdateAsync(CurrentUser);
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
            }
            else
            {
                return PartialView("_ChangePassword", model);
            }
            return PartialView("_ChangePassword", model);
        }

        [Authorize]
        public ActionResult ViewCredentials()
        {
            return PartialView("_ViewCredentials", CurrentUser);
        }

        [Authorize]
        public ActionResult ChangeCredentials()
        {
            return PartialView("_ChangeCredentials", CurrentUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangeCredentials(string id, string email, string firstName, string lastName)
        {
            var userToChange = await UserManager.FindByIdAsync(id);
            if (userToChange != null)
            {
                userToChange.Email = email;
                var validEmail = await UserManager.UserValidator.ValidateAsync(userToChange);
                if (!validEmail.Succeeded)
                {
                    AddModelErrors(validEmail);
                }
                else
                {
                    userToChange.FirstName = firstName;
                    userToChange.LastName = lastName;
                    var result = await UserManager.UpdateAsync(userToChange);
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
                var userToAdd = new AccUserModel()
                {
                    UserName = model.Login,
                    Email = model.Email,
                };
                var validMail = await UserManager.UserValidator.ValidateAsync(userToAdd);

                if (!validMail.Succeeded)
                {
                    AddModelErrors(validMail);
                }

                var validPass = await UserManager.PasswordValidator.ValidateAsync(model.Password);

                if (!validPass.Succeeded)
                {
                    AddModelErrors(validPass);
                }

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Введенные пароли не совпадают");
                }

                if (validPass.Succeeded && validMail.Succeeded && model.Password.Equals(model.ConfirmPassword))
                {
                    userToAdd.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                    var result = await UserManager.CreateAsync(userToAdd);

                    if (!result.Succeeded)
                    {
                        AddModelErrors(result);
                    }

                    result = await UserManager.AddToRoleAsync(userToAdd.Id, "Users");

                    if (!result.Succeeded)
                    {
                        AddModelErrors(result);
                    }
                    else
                    {
                        var address = HttpContext.Request.UserHostAddress;
                        await _userReporter.Log(userToAdd, address);

                        await SignInUser(userToAdd);

                        var urlToRedirect = Url.Action("Index", "PayingItem");
                        return Json(new { url = urlToRedirect, hasErrors = "false" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return PartialView("_Register", model);
            }

            return PartialView("_Register", model);
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private async void ActualizePlanItems(AccUserModel user)
        {
            try
            {
                await _planingHelper.ActualizePlanItems(user.Id);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(UserAccountController)} в методе {nameof(ActualizePlanItems)}",
                    e);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(UserAccountController)} в методе {nameof(ActualizePlanItems)}",
                    e);
            }
        }
    }
}