using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebUI.Core.Infrastructure.Identity.Models;
using WebUI.Core.Models.RoleModels;
using WebUI.Core.Models.UserModels;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class RoleAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AccountingUserModel> _userManager;

        public RoleAdminController(RoleManager<IdentityRole> roleManager, UserManager<AccountingUserModel> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var roles = _roleManager.Roles;
            var users = _userManager.Users;
            var rolesWithUsers = new List<RoleModel>();

            foreach (var role in roles)
            {
                var roleModel = new RoleModel()
                {
                    Id = role.Id,
                    Name = role.Name
                };                

                foreach (var user in users)
                {
                    var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                    
                    if (isInRole)
                    {
                        var userModel = new UserModel()
                        {
                            Id = user.Id,
                            Name = user.UserName
                        };

                        roleModel.Users.Add(userModel);
                    }
                }

                rolesWithUsers.Add(roleModel);
            }

            return PartialView("_Index", rolesWithUsers);
        }

        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Required(ErrorMessage = "Не указано название роли")] string rolename)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(rolename));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                AddErrorsToModel(result);
            } 

            return PartialView("_Create", rolename);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
            
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                return View("Error", result.Errors);
            }

            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var members = new List<AccountingUserModel>();
                var nonMembers = new List<AccountingUserModel>();

                foreach (var user in _userManager.Users)
                {
                    var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                    list.Add(user);
                }

                var roleEditModel = new RoleEditModel()
                {
                    Role = role,
                    Members = members,
                    NonMembers = nonMembers
                };

                return PartialView("_Edit", roleEditModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        //public async Task<ActionResult> Edit(RoleModificationModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        IdentityResult result;
        //        foreach (var userId in model.IdsToAdd ?? new string[] { })
        //        {
        //            result = await _userManager.AddToRoleAsync(userId, model.RoleName);

        //            if (!result.Succeeded)
        //            {
        //                return View("Error", result.Errors);
        //            }
        //        }
        //        foreach (var userId in model.IdsToDelete ?? new string[] { })
        //        {
        //            result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
        //            if (!result.Succeeded)
        //            {
        //                return View("Error", result.Errors);
        //            }
        //        }

        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}


        private void AddErrorsToModel(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }

}