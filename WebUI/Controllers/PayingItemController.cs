using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using WebUI.Abstract;
using WebUI.Exceptions;
using WebUI.Infrastructure.Attributes;
using WebUI.Models;
using WebUI.Models.CategoryModels;
using WebUI.Models.PayingItemModels;

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MainPage()
        {
            return PartialView("_MainPage");
        }

        public ActionResult List(WebUser user, int page = 1)
        {
            try
            {
                var items = _payingItemService.GetList(i => i.UserId == user.Id && i.Date >= DbFunctions.AddDays(DateTime.Today, -2)).ToList();
                var pItemToView = new PayingItemsCollectionModel()
                {
                    PayingItems = items
                        .OrderByDescending(i => i.Date)
                        .ThenBy(x => x.Category.Name)
                        .Skip((page - 1) * ItemsPerPage)
                        .Take(ItemsPerPage),
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = page,
                        ItemsPerPage = ItemsPerPage,
                        TotalItems = items.Count
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


        public ActionResult ListAjax(WebUser user, int page)
        {
            IEnumerable<PayingItem> items;
            try
            {
                var payingItems = _payingItemService.GetList(i => i.Date >= DbFunctions.AddDays(DateTime.Today, -2) && i.UserId == user.Id).ToList();
                items = payingItems
                    .OrderByDescending(i => i.Date)
                    .ThenBy(x => x.Category.Name)
                    .Skip((page - 1) * ItemsPerPage)
                    .Take(ItemsPerPage);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(ListAjax)}", e);
            }
            return PartialView("_PayingItemsPartial", items);
        }

        [UserHasCategories]
        [UserHasAnyAccount]
        public async Task<ActionResult> Add(WebUser user, int typeOfFlowId)
        {
            await FillViewBag(user, typeOfFlowId);
            var piModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { UserId = user.Id },
                Products = new List<Product>()
            };
            return PartialView("_Add", piModel);
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user, PayingItemModel model, int typeOfFlowId)
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
            await FillViewBag(user, typeOfFlowId);
            return PartialView("_Add", model);
        }

        public async Task<ActionResult> Edit(WebUser user, int typeOfFlowId, int id)
        {
            try
            {
                var payingItemEditModel = await _payingItemEditViewModelCreator.CreateViewModel(id);

                if (payingItemEditModel == null)
                {
                    return RedirectToAction("ListAjax", 1);
                }

                await FillViewBag(user, typeOfFlowId);

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
        public async Task<ActionResult> Edit(WebUser user, PayingItemEditModel model)
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

            await FillViewBag(user, await GetTypeOfFlowId(model.PayingItem));

            return PartialView("_Edit", model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(WebUser user, int id)
        {
            try
            {
                await _payingItemService.DeleteAsync(id);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Delete)}", e);
            }
            return RedirectToAction("List");
        }

        public ActionResult ExpensiveCategories(WebUser user)
        {
            List<PayingItem> tempList = null;
            try
            {
                tempList = _payingItemService.GetList()
                    .Where(x => x.UserId == user.Id && x.Category.TypeOfFlowID == 2 &&
                                x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year)
                    .ToList();
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(ExpensiveCategories)}", e);
            }

            var outList = (from item in tempList
                           group item by item.Category.Name
                    into grouping
                           select new CategorySumModel()
                           {
                               Category = grouping.Key,
                               Sum = grouping.Sum(x => x.Summ)
                           })
                .OrderByDescending(x => x.Sum)
                .Take(ItemsPerPage)
                .ToList();

            return PartialView("_ExpensiveCategories", outList);
        }

        public async Task<ActionResult> GetSubCategories(int id)
        {
            try
            {
                var products = (await _categoryService.GetItemAsync(id))
                    .Products
                    .OrderBy(x => x.ProductName)
                    .ToList();
                return PartialView("_GetSubCategories", products);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetSubCategories)}", e);
            }
        }

        public async Task<ActionResult> GetSubCategoriesForEdit(int id)
        {
            try
            {
                var products = (await _categoryService.GetItemAsync(id)).Products.ToList();
                return PartialView("_GetSubCategoriesForEdit", products);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetSubCategoriesForEdit)}", e);
            }
        }

        private async Task FillViewBag(WebUser user, int typeOfFlowId)
        {
            try
            {
                ViewBag.Categories = (await _categoryService.GetActiveGategoriesByUserAsync(user.Id))
                    .Where(i => i.TypeOfFlowID == typeOfFlowId)
                    .OrderBy(x => x.Name)
                    .ToList();
                ViewBag.Accounts = (await _accountService.GetListAsync())
                    .Where(x => x.UserId == user.Id)
                    .ToList();
            }
            catch (NullReferenceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(FillViewBag)}", e);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(FillViewBag)}", e);
            }
        }

        private async Task<int> GetTypeOfFlowId(PayingItem pItem)
        {
            try
            {
                return (await _categoryService.GetItemAsync(pItem.CategoryID)).TypeOfFlowID;
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetTypeOfFlowId)}", e);
            }
        }

        private bool CheckForSubCategories(PayingItem item)
        {
            try
            {
                return _payingItemService.GetList(x => x.CategoryID == item.CategoryID)
                    .Any();
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(CheckForSubCategories)}", e);
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