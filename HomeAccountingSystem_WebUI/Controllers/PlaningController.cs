using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Abstract;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.Default)]
    public class PlaningController : Controller
    {
        private readonly IRepository<Category> _catRepository;
        private readonly IRepository<PlanItem> _planItemRepository;
        private readonly IPlanningHelper _planningHelper;

        public PlaningController(IRepository<Category> catRepository,IRepository<PlanItem> planItemRepository, 
            IRepository<PayingItem> payingItemRepository, IPlanningHelper planningHelper)
        {
            _catRepository = catRepository;
            _planItemRepository = planItemRepository;
            _planningHelper = planningHelper;
        }

        public RedirectToRouteResult Prepare(WebUser user)
        {
            var categories = _catRepository.GetList().Any(x => x.UserId == user.Id);
            if (categories)
            {
                var planItems = _planItemRepository.GetList().Any(x => x.UserId == user.Id);
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

        public ActionResult ViewPlan(WebUser user)
        {
            if(_catRepository.GetList().Any(x => x.UserId == user.Id && x.ViewInPlan == true))
            {
                var model = _planningHelper.GetUserBalance(user, false);
                return View(model);
            }
            else
            {
                var model = _planningHelper.GetUserBalance(user, true);
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
                    await _planItemRepository.UpdateAsync(model.PlanItem);
                    await _planItemRepository.SaveAsync();
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

        public ActionResult SelectCategories(WebUser user)
        {
            var cats = GetUserCategories(user);
            return View(cats);
        }

        public async Task<ActionResult> SelectCategoriesAjax(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var cat = await _catRepository.GetItemAsync(id);
                cat.ViewInPlan = true;
                await _catRepository.UpdateAsync(cat);
            }
            _catRepository.GetList()
                .Where(x => !ids.Contains(x.CategoryID))
                .ToList()
                .ForEach(x=> { x.ViewInPlan = false; });
            await _catRepository.SaveAsync();
            return Json(new {url = Url.Action("ViewPlan")});
        }

        private async Task<EditPlaningModel> GetEditPlaningModel(int id)
        {
            var editPlanModel = new EditPlaningModel()
            {
                PlanItem = await _planItemRepository.GetItemAsync(id)
            };
            return editPlanModel;
        }

        private IList<Category> GetUserCategories(IWorkingUser user)
        {
            return _catRepository.GetList().Where(x=>x.UserId == user.Id).ToList();
        }
    }
}