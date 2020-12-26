using DomainModels.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Filters;
using WebUI.Core.Models;
using WebUI.Core.Models.CategoryModels;
using WebUI.Core.Models.PayingItemModels;

namespace WebUI.Core.Controllers
{
    [Authorize]
    public class PayingItemController : Controller
    {
        private readonly IPayingItemService _payingItemService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;
        private readonly IPayingItemCreator _payingItemCreator;
        private readonly IPayingItemEditViewModelCreator _payingItemEditViewModelCreator;
        private readonly IPayingItemUpdater _payingItemUpdater;
        public int ItemsPerPage = 10;

        public PayingItemController(
            IPayingItemService payingItemService,
            ICategoryService categoryService,
            IAccountService accountService,
            IPayingItemCreator payingItemCreator,
            IPayingItemEditViewModelCreator payingItemEditViewModelCreator,
            IPayingItemUpdater payingItemUpdater)
        {
            _payingItemService = payingItemService;
            _categoryService = categoryService;
            _accountService = accountService;
            _payingItemCreator = payingItemCreator;
            _payingItemEditViewModelCreator = payingItemEditViewModelCreator;
            _payingItemUpdater = payingItemUpdater;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MainPage()
        {
            return PartialView("_MainPage");
        }

        public async Task<IActionResult> List(WebUser user, int page = 1)
        {
            try
            {
                var dateToday = DateTime.Now.Date;
                var dateMinusTwoDays = DateTime.Now.Date - TimeSpan.FromDays(2);
                var taskResult = await _payingItemService.GetListAsync(i => i.UserId == user.Id && (i.Date >= dateMinusTwoDays && i.Date <= dateToday));
                var payingItems = taskResult.ToList();
                var pItemToView = new PayingItemsListWithPaginationModel()
                {
                    PayingItems = payingItems
                        .OrderByDescending(i => i.Date)
                        .ThenBy(x => x.Category.Name)
                        .Skip((page - 1) * ItemsPerPage)
                        .Take(ItemsPerPage),                        
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = page,
                        ItemsPerPage = ItemsPerPage,
                        TotalItems = payingItems.Count
                    }
                };

                return PartialView("_List", pItemToView);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(List)}",
                    e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(List)}",
                    e);
            }
        }

        public async Task<IActionResult> ListAjax(WebUser user, int page)
        {
            try
            {
                var dateToday = DateTime.Now.Date;
                var dateMinusTwoDays = DateTime.Now.Date - TimeSpan.FromDays(2);
                var taskResult = await _payingItemService.GetListAsync(i => i.UserId == user.Id && (i.Date >= dateMinusTwoDays && i.Date <= dateToday));
                var payingItems = taskResult.ToList();
                var items = payingItems
                    .OrderByDescending(i => i.Date)
                    .ThenBy(x => x.Category.Name)
                    .Skip((page - 1) * ItemsPerPage)
                    .Take(ItemsPerPage);

                return PartialView("_PayingItems", items);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(ListAjax)}", e);
            }
        }

        [TypeFilter(typeof(UserHasCategories))]
        [TypeFilter(typeof(UserHasAnyAccount))]
        public async Task<IActionResult> Add(WebUser user, int typeOfFlowId)
        {
            await FillViewBagWithCategoriesAndAccounts(user, typeOfFlowId);

            var piModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { UserId = user.Id, Date = DateTime.Today },
                Products = new List<Product>()
            };

            return PartialView("_Add", piModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(WebUser user, PayingItemModel model, int typeOfFlowId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _payingItemCreator.CreatePayingItemFromViewModel(model);

                    return RedirectToAction("List");
                }
                catch (WebUiException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Add)}", e);
                }
                catch (Exception e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Add)}", e);
                }
            }

            await FillViewBagWithCategoriesAndAccounts(user, typeOfFlowId);
            return PartialView("_Add", model);
        }

        public async Task<IActionResult> Edit(WebUser user, int typeOfFlowId, int id)
        {
            try
            {
                var payingItemEditModel = await _payingItemEditViewModelCreator.CreateViewModel(id);

                if (payingItemEditModel == null)
                {
                    return RedirectToAction("ListAjax", 1);
                }

                await FillViewBagWithCategoriesAndAccounts(user, typeOfFlowId);

                return PartialView("_Edit", payingItemEditModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
            }
            catch (WebUiException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WebUser user, PayingItemEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _payingItemUpdater.UpdatePayingItemFromViewModel(model);

                    return RedirectToAction("List");
                }
                catch (ServiceException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
                }
                catch (Exception e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
                }
            }

            await FillViewBagWithCategoriesAndAccounts(user, await GetTypeOfFlowId(model.PayingItem));

            return PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(WebUser user, int id)
        {
            try
            {
                await _payingItemService.DeleteAsync(id);

                return RedirectToAction("List");
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Delete)}", e);
            }
        }

        public async Task<IActionResult> GetSubCategories(int id)
        {
            try
            {
                var payingItem = await _categoryService.GetItemAsync(id); 
                var payingItemProducts = payingItem
                    .Products
                    .OrderBy(x => x.ProductName)
                    .ToList();

                return PartialView("_GetSubCategories", payingItemProducts);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetSubCategories)}", e);
            }
        }

        public async Task<IActionResult> GetSubCategoriesForEdit(int id)
        {
            try
            {
                var category = await _categoryService.GetItemAsync(id);
                var products = category.Products.ToList();

                return PartialView("_GetSubCategoriesForEdit", products);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetSubCategoriesForEdit)}", e);
            }
        }

        private async Task FillViewBagWithCategoriesAndAccounts(WebUser user, int typeOfFlowId)
        {
            try
            {
                var activeCategoriesTaskResult = await _categoryService.GetActiveGategoriesByUserAsync(user.Id);
                ViewBag.Categories = activeCategoriesTaskResult
                    .Where(i => i.TypeOfFlowID == typeOfFlowId)
                    .OrderBy(x => x.Name)
                    .ToList();

                var getAccountTaskResult = await _accountService.GetListAsync(x => x.UserId == user.Id);
                ViewBag.Accounts = getAccountTaskResult.ToList();
            }
            catch (NullReferenceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(FillViewBagWithCategoriesAndAccounts)}", e);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(FillViewBagWithCategoriesAndAccounts)}", e);
            }
        }

        private async Task<int> GetTypeOfFlowId(PayingItem pItem)
        {
            try
            {
                var payingItem = await _categoryService.GetItemAsync(pItem.CategoryID);

                return payingItem.TypeOfFlowID;
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetTypeOfFlowId)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _payingItemService.Dispose();
            _categoryService.Dispose();
            _accountService.Dispose();
            _payingItemCreator.Dispose();
            _payingItemEditViewModelCreator.Dispose();
            _payingItemUpdater.Dispose();

            base.Dispose(disposing);
        }
    }
}