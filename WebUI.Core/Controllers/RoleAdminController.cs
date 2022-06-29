using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Identity.Models;
using WebUI.Core.Models.Enums;
using WebUI.Core.Models.RoleModels;
using WebUI.Core.Models.UserModels;

namespace WebUI.Core.Controllers
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
        public async Task<ActionResult> Create([Required(ErrorMessage = "Не указано название роли")] string roleName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    AddErrorsToModel(result);
                }
                catch (Exception e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Create)}", e);
                }
            } 

            return PartialView("_Create", roleName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
            
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
            
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    AddErrorsToModel(result);

                    return await Index();
                }

                ModelState.AddModelError("",$"Роль с Id {id} не найдена");

                return await Index();
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Delete)}", e);
            }
        }


        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);

                if (role != null)
                {
                    var members = new List<UserModel>();
                    var nonMembers = new List<UserModel>();

                    foreach (var user in _userManager.Users)
                    {
                        var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                        list.Add(new UserModel() { Id = user.Id, Name = user.UserName });
                    }

                    var roleEditModel = new RoleEditModel()
                    {
                        Role = new RoleModel() { Id = role.Id, Name = role.Name },
                        Members = members,
                        NonMembers = nonMembers
                    };

                    return PartialView("_Edit", roleEditModel);
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Edit)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            try
            {
                await AddOrRemoveUsersFromRole(model.UsersToAdd, model.RoleName, UserActions.AddToRole);
                await AddOrRemoveUsersFromRole(model.UsersToDelete, model.RoleName, UserActions.RemoveFromRole);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }

                return await Edit(model.RoleId);

            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(OrderController)} в методе {nameof(Edit)}", e);
            }
        }


        private async Task AddOrRemoveUsersFromRole(IEnumerable<string> userIds, string roleName, UserActions userActions)
        {
            foreach (var userId in userIds ?? new string[] { })
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    IdentityResult result;
                    switch (userActions)
                    {
                        case UserActions.RemoveFromRole:
                            result = await _userManager.RemoveFromRoleAsync(user, roleName);
                            break;
                        case UserActions.AddToRole:
                            result = await _userManager.AddToRoleAsync(user, roleName);
                            break;
                        default: result = IdentityResult.Failed();
                            break;
                    }

                    if (!result.Succeeded)
                    {
                        AddErrorsToModel(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"Невозможно добавить/удалить пользователя с Id {userId}, так как он не существует");
                }
            }
        }

        private void AddErrorsToModel(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }

}