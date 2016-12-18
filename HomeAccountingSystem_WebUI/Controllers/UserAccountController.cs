using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Infrastructure;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Runtime.Caching;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    public class UserAccountController : Controller
    {
        private AccUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<AccUserManager>();
        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;
        private AccUserModel CurrentUser => UserManager.FindById(HttpContext.User.Identity.GetUserId());
        private readonly IReporter _userReporter;
        private readonly IPlanningHelper _planingHelper;

        public UserAccountController(IReporter userReporter, IPlanningHelper planingHelper)
        {
            _userReporter = userReporter;
            _planingHelper = planingHelper;
        }

        [AllowAnonymous]
        public async Task<ActionResult> LoginDemo()
        {
            var loginModel = new LoginModel()
            {
                Name = "demo",
                Password = "12qw34er"
            };
            return await Login(loginModel, "/");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] {"Доступ запрещен"});
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                    AccUserModel user = await UserManager.FindAsync(model.Name, model.Password);
                    if (user != null)
                    {
                        ClaimsIdentity identity =
                            await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthManager.SignOut();
                        AuthManager.SignIn(new AuthenticationProperties() {IsPersistent = false}, identity);
                        if (user.UserName == "demo")
                            {
                                var address = HttpContext.Request.UserHostAddress;
                                new Thread(() => _userReporter.Report(user, address)).Start();
                            }
                        new Thread(() => ActualizePlanItems(user)).Start();
                        
                        Session["WebUser"] = new WebUser() { Id = user.Id, Name = user.UserName, Email = user.Email};

                    return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверные имя пользователя или пароль");
                    }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

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
            return View();
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
                    ModelState.AddModelError("","Неверный текущий пароль");
                    return View(model);
                }
                if (pass == PasswordVerificationResult.Success)
                {
                    if (!model.NewPassword.Equals(model.ConfirmPassword))
                    {
                        ModelState.AddModelError("","Введенные пароли не сопадают");
                        return View(model);
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
                                TempData["message"] = "Пароль успешно изменен";
                                return RedirectToAction("ViewCredentials");
                            }
                        }
                    }
                }
            }
            else
            {
                return View(model);
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ViewCredentials()
        {
            return View(CurrentUser);
        }

        [Authorize]
        public ActionResult ChangeCredentials()
        {
            return View(CurrentUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangeCredentials(string id,string email,string firstName, string lastName)
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
                        TempData["message"] = "Учетные данные успешно изменены";
                        return RedirectToAction("ViewCredentials");
                    }
                }
            }
            else
            {
                return View("Error",new string[] {"Ошибка при изменении данных"});
            }
            return View(userToChange);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
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
                    ModelState.AddModelError("","Введенные пароли не совпадают");
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
                        new Thread( () => _userReporter.Report(userToAdd, address)).Start();
                        TempData["message"] = "Спасибо за регистрацию. Теперь можно войти в систему со своим логином и паролем";
                        return Redirect("/");
                    }
                }
            }
            else
            {
                return View(model);
            }
            return View(model);
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("",error);
            }
        }

        private async void ActualizePlanItems(AccUserModel user)
        {
            await _planingHelper.ActualizePlanItems(user.Id);
        }
    }
}