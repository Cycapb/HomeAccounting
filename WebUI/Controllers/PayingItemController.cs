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

namespace WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PayingItemController : Controller
    {        
        private readonly IPayingItemHelper _payingItemHelper;
        private readonly IPayingItemService _payingItemService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;

        public int ItemsPerPage = 10;

        public PayingItemController(            
            IPayingItemHelper payingItemHelper,
            IPayingItemService payingItemService,
            ICategoryService categoryService,
            IAccountService accountService)
        {
            _payingItemHelper = payingItemHelper;
            _payingItemService = payingItemService;
            _categoryService = categoryService;
            _accountService = accountService;            
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
            PayingItemToView pItemToView;
            try
            {                
                var items = _payingItemService.GetList(i => i.UserId == user.Id && i.Date >= DbFunctions.AddDays(DateTime.Today, -2)).ToList();
                pItemToView = new PayingItemToView()
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
            return PartialView("_List", pItemToView);
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

        [UserHasCategoriesAttribute]
        [UserHasAnyAccount]
        public async Task<ActionResult> Add(WebUser user, int typeOfFlow)
        {
            await FillViewBag(user, typeOfFlow);
            var piModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() { UserId = user.Id },
                Products = new List<Product>()
            };
            return PartialView("_Add", piModel);
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user, PayingItemModel model, int typeOfFlow)
        {
            if (ModelState.IsValid)
            {
                if (model.PayingItem.Date.Month > DateTime.Today.Date.Month ||
                    model.PayingItem.Date.Year > DateTime.Today.Year)
                {
                    model.PayingItem.Date = DateTime.Today.Date;
                }
                try
                {
                    if (model.Products == null)
                    {
                        await _payingItemService.CreateAsync(model.PayingItem);
                    }
                    else
                    {
                        var sum = model.Products.Sum(x => x.Price);
                        if (sum != 0)
                        {
                            model.PayingItem.Summ = sum;
                        }
                        _payingItemHelper.CreateCommentWhileAdd(model);
                        _payingItemHelper.CreatePayingItemProducts(model);

                        await _payingItemService.CreateAsync(model.PayingItem);
                    }
                }
                catch (ServiceException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Add)}", e);
                }
                catch (WebUiHelperException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Add)}", e);
                }
                catch (Exception e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Add)}", e);
                }
                return RedirectToAction("List");
            }
            await FillViewBag(user, typeOfFlow);
            return PartialView("_Add", model);
        }

        public async Task<ActionResult> Edit(WebUser user, int typeOfFlowId, int id)
        {
            await FillViewBag(user, typeOfFlowId);
            try
            {
                var payingItem = await _payingItemService.GetItemAsync(id);

                if (payingItem == null)
                {
                    return RedirectToAction("ListAjax", 1);
                }

                var payingItemEditModel = new PayingItemEditModel()
                {
                    PayingItem = payingItem,
                    PayingItemProducts = new List<PaiyngItemProduct>()
                };
                PayingItemEditModel.OldCategoryId = payingItem.CategoryID;

                if (!CheckForSubCategories(payingItem))
                {
                    return PartialView("_Edit", payingItemEditModel);
                }
                _payingItemHelper.FillPayingItemEditModel(payingItemEditModel, id);
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
            catch (WebUiHelperException e)
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
                    if (model.PricesAndIdsInItem == null)
                    {
                        await _payingItemService.UpdateAsync(model.PayingItem);
                    }
                    else
                    {
                        var sum = GetSumForPayingItem(model);

                        if (sum != 0 )
                        {
                            model.PayingItem.Summ = sum;
                        }
                        
                        _payingItemHelper.CreateCommentWhileEdit(model);                        

                        if (PayingItemEditModel.OldCategoryId != model.PayingItem.CategoryID)
                        {
                            await _payingItemHelper.CreatePayingItemProducts(model);                            
                        }
                        else
                        {
                            await _payingItemHelper.UpdatePayingItemProducts(model);
                        }
                    }

                    await _payingItemService.UpdateAsync(model.PayingItem);
                }
                catch (ServiceException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
                }
                catch (WebUiHelperException e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
                }
                catch (Exception e)
                {
                    throw new WebUiException(
                        $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Edit)}", e);
                }
                return RedirectToAction("List");
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
                           select new OverAllItem()
                           {
                               Category = grouping.Key,
                               Summ = grouping.Sum(x => x.Summ)
                           })
                .OrderByDescending(x => x.Summ)
                .Take(ItemsPerPage)
                .ToList();

            return PartialView("_ExpensiveCategories", outList);
        }

        public async Task<ActionResult> GetSubCategories(int id)
        {
            try
            {
                var products = (await _categoryService.GetItemAsync(id))
                    .Product
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
                var products = (await _categoryService.GetProducts(id)).ToList();
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
                ViewBag.Categories = (await _categoryService.GetActiveGategoriesByUser(user.Id))
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

        private decimal GetSumForPayingItem(PayingItemEditModel model)
        {
            if (model.PricesAndIdsNotInItem == null)
            {
                return model.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price);
            }
            return model.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price) +
                   model.PricesAndIdsNotInItem.Where(x => x.Id != 0).Sum(x => x.Price);
        }
    }
}