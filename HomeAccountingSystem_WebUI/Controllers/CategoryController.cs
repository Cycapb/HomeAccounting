using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using DomainModels.Model;
using WebUI.Abstract;
using WebUI.Models;
using Services;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CategoryController : Controller
    {
        private readonly ITypeOfFlowService _tofService;
        private readonly IPlanningHelper _planningHelper;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryHelper _categoryHelper;
        private readonly int _pagesize = 7;

        public CategoryController( ITypeOfFlowService tofService,
            IPlanningHelper planningHelper, ICategoryService categoryService, ICategoryHelper categoryHelper)
        {
            _tofService = tofService;
            _planningHelper = planningHelper;
            _categoryService = categoryService;
            _categoryHelper = categoryHelper;
        }

        public async Task<ActionResult> Index(WebUser user, int page = 1)
        {
            var catView = await _categoryHelper.CreateCategoriesViewModel(page, _pagesize, x => x.UserId == user.Id);
            return PartialView(catView);
        }

        public async Task<ActionResult> GetAllCategories(WebUser user, int page = 1)
        {
            var categories = await _categoryHelper.GetCategoriesToShowOnPage(page,_pagesize, x => x.UserId == user.Id);
            return PartialView("CategorySummaryPartial",categories);
        }

        public async Task<ActionResult> GetCategoriesByType(WebUser user, int typeOfFlowId, int page)
        {
            var categories = await _categoryHelper.GetCategoriesToShowOnPage(page, _pagesize, x => x.UserId == user.Id && x.TypeOfFlowID == typeOfFlowId);
            return PartialView("CategorySummaryPartial",categories);
        }

        public async Task<ActionResult> GetCategoriesAndPagesByType(WebUser user, int typeOfFlowId, int page)
        {
            var model = await _categoryHelper.CreateCategoriesViewModel(page, _pagesize, x => x.UserId == user.Id && x.TypeOfFlowID == typeOfFlowId);
            model.TypeOfFlowId = typeOfFlowId;

            return PartialView(model);
        }

        public async Task<ActionResult> GetCategoriesAndPages(WebUser user, int page = 1)
        {
            var model = await _categoryHelper.CreateCategoriesViewModel(page, _pagesize, c => c.UserId == user.Id);
            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.TypesOfFlow = await GetTypesOfFlow();
            var item = await _categoryService.GetItemAsync(id);
            if (item == null)
            {
                return RedirectToAction("Index");
            }
            return PartialView(item);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateAsync(category);
                await _categoryService.SaveAsync();
                return RedirectToAction("GetCategoriesAndPages");
            }
            else
            {
                ViewBag.TypesOfFlow = await GetTypesOfFlow();
                return PartialView(category);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Add(WebUser user)
        {
            ViewBag.TypesOfFlow = await GetTypesOfFlow();
            return PartialView(new Category() {UserId = user.Id});
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user, Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateAsync(category);
                await _planningHelper.CreatePlanItemsForCategory(user, category.CategoryID);
                return RedirectToAction("GetCategoriesAndPages");
            }
            else
            {
                ViewBag.TypesOfFlow = await GetTypesOfFlow();
                return PartialView(category);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            if (! await _categoryService.HasDependencies(id))
            {
                await _categoryService.DeleteAsync(id);
            }
            return RedirectToAction("GetCategoriesAndPages");
        }

        private async Task<IEnumerable<TypeOfFlow>> GetTypesOfFlow()
        {
            return (await _tofService.GetListAsync()).ToList();
        }
    }
}