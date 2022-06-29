using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Models;
using WebUI.Core.Models.PlanningModels;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class PlanningController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IPlanItemService _planItemService;
        private readonly IPlanningHelper _planningHelper;

        public PlanningController(
            ICategoryService categoryService,
            IPlanItemService planItemService, 
            IPlanningHelper planningHelper)
        {
            _categoryService = categoryService;
            _planItemService = planItemService;
            _planningHelper = planningHelper;
        }

        public async Task<IActionResult> Prepare(WebUser user)
        {
            var userHasCategories = (await _categoryService.GetListAsync()).Any(x => x.UserId == user.Id);
            
            if (userHasCategories)
            {
                var userHasPlanItems = (await _planItemService.GetListAsync(x => x.UserId == user.Id)).Any();
                
                return RedirectToAction(userHasPlanItems ? "ViewPlan" : "CreatePlan");
            }

            TempData["message"] = "Сначала необходимо добавить категории, по которым Вы будете делать планирование";
            
            return RedirectToAction("Index", "PayingItem");
        }

        public async Task<IActionResult> CreatePlan(WebUser user)
        {
            await _planningHelper.CreatePlanItems(user);
            
            return RedirectToAction("ViewPlan");
        }

        public async Task<IActionResult> ViewPlan(WebUser user)
        {
            var userHasCategoriesToViewInPlan = (await _categoryService.GetActiveGategoriesByUserAsync(user.Id))
                .Any(x => x.ViewInPlan);
            
            if(userHasCategoriesToViewInPlan)
            {
                var model = await _planningHelper.GetUserBalance(user, false);
                
                return View(model);
            }
            else
            {
                var model = await _planningHelper.GetUserBalance(user, true);
                
                return View(model);
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            return PartialView(await GetEditPlaningModel(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(WebUser user, PlanningEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.Spread)
                {
                    await _planItemService.UpdateAsync(model.PlanItem);
                }
                else
                {
                    await _planningHelper.SpreadPlanItems(user, model.PlanItem);
                }
                
                // if (Request.IsAjaxRequest())
                // {
                //     return Json(new { url = Url.Action("ViewPlan") });
                // }

                return RedirectToAction("ViewPlan");

            }
            return View(model);
        }

        public async Task<IActionResult> EditView(int id)
        {
            return View(await GetEditPlaningModel(id));
        }

        public async Task<IActionResult> ActualizePlanItems(WebUser user)
        {
            await _planningHelper.ActualizePlanItems(user.Id);
            
            return Json(new { url = Url.Action("ViewPlan") });
        }

        public async Task<IActionResult> Actualize(WebUser user)
        {
            await _planningHelper.ActualizePlanItems(user.Id);
            
            return RedirectToAction("ViewPlan");
        }

        public async Task<IActionResult> SelectCategories(WebUser user)
        {
            var categories = await _categoryService.GetActiveGategoriesByUserAsync(user.Id);
            
            return View(categories.ToList());
        }

        public async Task<IActionResult> SelectCategoriesAjax(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var cat = await _categoryService.GetItemAsync(id);
                cat.ViewInPlan = true;
                await _categoryService.UpdateAsync(cat);
            }
            
            var categories = await _categoryService.GetListAsync();
                categories.Where(x => !ids.Contains(x.CategoryID))
                .ToList()
                .ForEach(x =>
                {
                    x.ViewInPlan = false;
                });
            
            return Json(new {url = Url.Action("ViewPlan")});
        }

        private async Task<PlanningEditModel> GetEditPlaningModel(int id)
        {
            var editPlanModel = new PlanningEditModel()
            {
                PlanItem = await _planItemService.GetItemAsync(id)
            };
            
            return editPlanModel;
        }
    }
}