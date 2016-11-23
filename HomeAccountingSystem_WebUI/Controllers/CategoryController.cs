using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CategoryController : Controller
    {
        private readonly IRepository<TypeOfFlow> _tofRepository;
        private readonly IPlanningHelper _planningHelper;
        private readonly ICategoryService _categoryService;
        private readonly int _pagesize = 7;

        public CategoryController(IRepository<TypeOfFlow> tofRepo, 
            IPlanningHelper planningHelper, ICategoryService categoryService)
        {
            _tofRepository = tofRepo;
            _planningHelper = planningHelper;
            _categoryService = categoryService;
        }

        public async Task<ActionResult> Index(WebUser user, int page = 1)
        {
            var catView = new CategoriesViewModel()
            {
                Categories = (await _categoryService.GetListAsync())
                    .Where(x => x.UserId == user.Id)
                    .Skip((page - 1)*_pagesize)
                    .Take(_pagesize)
                    .ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    TotalItems = (await _categoryService.GetListAsync())
                        .Count(x => x.UserId == user.Id),
                    ItemsPerPage = _pagesize
                }
            };
            return PartialView(catView);
        }

        public async Task<ActionResult> GetAllCategories(WebUser user, int page = 1)
        {
            var categories = (await _categoryService.GetListAsync())
                .Where(x => x.UserId == user.Id)
                .Skip((page - 1)*_pagesize)
                .Take(_pagesize)
                .ToList();
            return PartialView("CategorySummaryPartial",categories);
        }

        public async Task<ActionResult> GetCategoriesByType(WebUser user, int typeOfFlowId, int page)
        {
            var categories = (await _categoryService.GetListAsync())
                .Where(x => x.UserId == user.Id && x.TypeOfFlowID == typeOfFlowId)
                .Skip((page - 1)*_pagesize)
                .Take(_pagesize)
                .ToList();
            return PartialView("CategorySummaryPartial",categories);
        }

        public async Task<ActionResult> GetCategoriesAndPagesByType(WebUser user, int typeOfFlowId, int page)
        {
            var model = new CategoriesViewModel()
            {
                Categories = (await _categoryService.GetListAsync())
                    .Where(x => x.UserId == user.Id && x.TypeOfFlowID == typeOfFlowId)
                    .Skip((page - 1)*_pagesize)
                    .Take(_pagesize)
                    .ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    TotalItems = (await _categoryService.GetListAsync())
                        .Count(x => x.UserId == user.Id && x.TypeOfFlowID == typeOfFlowId),
                    ItemsPerPage = _pagesize
                },
                TypeOfFlowId = typeOfFlowId
            };
            return PartialView(model);
        }

        public async Task<ActionResult> GetCategoriesAndPages(WebUser user, int page = 1)
        {
            var model = new CategoriesViewModel()
            {
                Categories = (await _categoryService.GetListAsync())
                    .Where(x => x.UserId == user.Id)
                    .Skip((page - 1) * _pagesize)
                    .Take(_pagesize)
                    .ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    TotalItems = (await _categoryService.GetListAsync())
                        .Count(x => x.UserId == user.Id),
                    ItemsPerPage = _pagesize
                }
            };
            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.TypesOfFlow = _tofRepository.GetList().ToList();
            var item = await _categoryService.GetItemAsync(id);
            return PartialView(item);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateAsync(category);
                return RedirectToAction("GetCategoriesAndPages");
            }
            else
            {
                ViewBag.TypesOfFlow = _tofRepository.GetList().ToList();
                return PartialView(category);
            }
        }

        [HttpGet]
        public ActionResult Add(WebUser user)
        {
            ViewBag.TypesOfFlow = _tofRepository.GetList().ToList();
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
                ViewBag.TypesOfFlow = _tofRepository.GetList().ToList();
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
    }
}