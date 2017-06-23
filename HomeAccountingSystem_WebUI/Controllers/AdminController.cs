using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using WebUI.Infrastructure;
using WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    [SessionState(SessionStateBehavior.Disabled)]
    public class AdminController : Controller
    {
        private AccUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<AccUserManager>();
        
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AccUserModel() {Email = model.Email,UserName = model.Name};
                IdentityResult result = await UserManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddModelErrors(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var userToDelete = await UserManager.FindByIdAsync(id);
            if (userToDelete != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(userToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error",result.Errors);
                }
            }
            return View("Error", new string[] { "Пользователь не найден"});
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var userModel = new EditModel()
                {
                    Id = user.Id,
                    Email = user.Email
                };
                return View(userModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditModel model)
        {
            var accUser = await UserManager.FindByIdAsync(model.Id);
            if (accUser != null)
            {
                accUser.Email = model.Email;
                var  validMail= await UserManager.UserValidator.ValidateAsync(accUser);
                if (!validMail.Succeeded)
                {
                    AddModelErrors(validMail);
                }
                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    validPassword = await UserManager.PasswordValidator.ValidateAsync(model.Password);
                    if (!validPassword.Succeeded)
                    {
                        AddModelErrors(validPassword);
                    }
                    else
                    {
                        accUser.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                    }
                }
                if ((validMail.Succeeded && validPassword == null) || (validMail.Succeeded && validPassword.Succeeded && model.Password != String.Empty))
                {
                    IdentityResult result = await UserManager.UpdateAsync(accUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddModelErrors(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("","Пользователь не найден");
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
    }
}