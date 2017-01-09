using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Default)]
    public class PlaningController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IPlanItemService _planItemService;
        private readonly IPlanningHelper _planningHelper;

        public PlaningController(ICategoryService categoryService,IPlanItemService planItemService, 
             IPlanningHelper planningHelper)
        {
            _categoryService = categoryService;
            _planItemService = planItemService;
            _planningHelper = planningHelper;
        }

        public async Task<ActionResult> Prepare(WebUser user)
        {
            var categories = (await _categoryService.GetListAsync()).Any(x => x.UserId == user.Id);
            if (categories)
            {
                var planItems = (await _planItemService.GetListAsync(user.Id)).Any();
                return RedirectToAction(planItems ? "ViewPlan" : "CreatePlan");
            }
            else
            {
                TempData["message"] = "Сначала необходимо добавить категории, по которым Вы будете делать планирование";
                return RedirectToAction("Index", "PayingItem");
            }
        }

        public async Task<RedirectToRouteResult> CreatePlan(WebUser user)
        {
            await _planningHelper.CreatePlanItems(user);
            return RedirectToAction("ViewPlan");
        }

        public async Task<ActionResult> ViewPlan(WebUser user)
        {
            if((await _categoryService.GetActiveGategoriesByUser(user.Id)).Any(x => x.ViewInPlan == true))
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

        public async Task<ActionResult> Edit(int id)
        {
            return PartialView(await GetEditPlaningModel(id));
        }

        public async Task<ActionResult> EditView(int id)
        {
            return View(await GetEditPlaningModel(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(WebUser user,EditPlaningModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.Spread)
                {
                    await _planItemService.UpdateAsync(model.PlanItem);
                    await _planItemService.SaveAsync();
                }
                else
                {
                    await _planningHelper.SpreadPlanItems(user, model.PlanItem);
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(new { url = Url.Action("ViewPlan") });
                }
                else
                {
                    return RedirectToAction("ViewPlan");
                }
                
            }
            return View(model);
        }

        public async Task<ActionResult> ActualizePlanItems(WebUser user)
        {
            await _planningHelper.ActualizePlanItems(user.Id);
            return Json(new { url = Url.Action("ViewPlan") });
        }

        public async Task<ActionResult> Actualize(WebUser user)
        {
            await _planningHelper.ActualizePlanItems(user.Id);
            return RedirectToAction("ViewPlan");
        }

        public async Task<ActionResult> SelectCategories(WebUser user)
        {
            var cats = await GetUserCategories(user);
            return View(cats);
        }

        public async Task<ActionResult> SelectCategoriesAjax(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var cat = await _categoryService.GetItemAsync(id);
                cat.ViewInPlan = true;
                await _categoryService.UpdateAsync(cat);
            }
            (await _categoryService.GetListAsync())
                .Where(x => !ids.Contains(x.CategoryID))
                .ToList()
                .ForEach(x =>
                {
                    x.ViewInPlan = false;
                });
            await _categoryService.SaveAsync();
            
            return Json(new {url = Url.Action("ViewPlan")});
        }

        private async Task<EditPlaningModel> GetEditPlaningModel(int id)
        {
            var editPlanModel = new EditPlaningModel()
            {
                PlanItem = await _planItemService.GetItemAsync(id)
            };
            return editPlanModel;
        }

        private async Task<IList<Category>> GetUserCategories(WebUser user)
        {
            return (await _categoryService.GetActiveGategoriesByUser(user.Id)).ToList();
        }
    }
}