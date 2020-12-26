using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;
using Services.Exceptions;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ITypeOfFlowService _tofService;
        private readonly IPlanningHelper _planningHelper;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryHelper _categoryHelper;
        private readonly int _pagesize = 7;

        public CategoryController(
            ITypeOfFlowService tofService,
            IPlanningHelper planningHelper,
            ICategoryService categoryService, 
            ICategoryHelper categoryHelper)
        {
            _tofService = tofService;
            _planningHelper = planningHelper;
            _categoryService = categoryService;
            _categoryHelper = categoryHelper;
        }

        public async Task<ActionResult> Index(WebUser user, int page = 1)
        {
            try
            {
                var categoriesViewModel = await _categoryHelper.CreateCategoriesViewModel(page, _pagesize, x => x.UserId == user.Id);

                return PartialView(categoriesViewModel);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(Index)}", e);
            }
        }

        public async Task<ActionResult> GetAllCategories(WebUser user, int page = 1)
        {
            try
            {
                var categories = await _categoryHelper.GetCategoriesToShowOnPage(page, _pagesize, x => x.UserId == user.Id);

                return PartialView("CategorySummaryPartial", categories);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(GetAllCategories)}", e);
            }
        }

        public async Task<ActionResult> GetCategoriesByType(WebUser user, int typeOfFlowId, int page)
        {
            try
            {
                var categories = await _categoryHelper.GetCategoriesToShowOnPage(page, _pagesize, x => x.UserId == user.Id && x.TypeOfFlowID == typeOfFlowId);

                return PartialView("CategorySummaryPartial", categories);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(GetCategoriesByType)}", e);
            }
        }

        public async Task<ActionResult> GetCategoriesAndPagesByType(WebUser user, int typeOfFlowId, int page)
        {
            try
            {
                var categoriesViewModel = await _categoryHelper.CreateCategoriesViewModel(page, _pagesize, x => x.UserId == user.Id && x.TypeOfFlowID == typeOfFlowId);
                categoriesViewModel.TypeOfFlowId = typeOfFlowId;

                return PartialView(categoriesViewModel);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(GetCategoriesAndPagesByType)}", e);
            }
        }

        public async Task<ActionResult> GetCategoriesAndPages(WebUser user, int page = 1)
        {
            try
            {
                var categoriesViewModel = await _categoryHelper.CreateCategoriesViewModel(page, _pagesize, c => c.UserId == user.Id);

                return PartialView(categoriesViewModel);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(GetCategoriesAndPages)}", e);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ViewBag.TypesOfFlow = await GetTypesOfFlow();
                var category = await _categoryService.GetItemAsync(id);

                if (category == null)
                {
                    return RedirectToAction("Index");
                }

                return PartialView(category);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(Edit)}", e);
            }            
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateAsync(category);

                    return RedirectToAction("GetCategoriesAndPages");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(Edit)}", e);
                }
            }

            ViewBag.TypesOfFlow = await GetTypesOfFlow();

            return PartialView(category);
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
                try
                {
                    await _categoryService.CreateAsync(category);
                    await _planningHelper.CreatePlanItemsForCategory(user, category.CategoryID);

                    return RedirectToAction("GetCategoriesAndPages");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException($"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(Add)}", e);
                }                
            }

            ViewBag.TypesOfFlow = await GetTypesOfFlow();
            return PartialView(category);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (!await _categoryService.HasDependenciesAsync(id))
                {
                    await _categoryService.DeleteAsync(id);
                }

                return RedirectToAction("GetCategoriesAndPages");
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(Delete)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _categoryService.Dispose();
            _tofService.Dispose();
            _categoryHelper.Dispose();

            base.Dispose(disposing);
        }

        private async Task<IEnumerable<TypeOfFlow>> GetTypesOfFlow()
        {
            try
            {
                return (await _tofService.GetListAsync()).ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(CategoryController)} в методе {nameof(GetTypesOfFlow)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка {e.GetType()} в контроллере {nameof(CategoryController)} в методе {nameof(GetTypesOfFlow)}", e);
            }
        }
    }
}