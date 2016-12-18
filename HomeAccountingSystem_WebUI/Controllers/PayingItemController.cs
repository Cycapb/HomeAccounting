using System;
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
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PayingItemController : Controller
    {
        private readonly IPayingItemProductHelper _pItemProductHelper;
        private readonly IPayingItemHelper _payingItemHelper;
        private readonly IPayingItemService _payingItemService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;

        public int ItemsPerPage = 10;
        
        public PayingItemController( 
            IPayingItemProductHelper payingItemProductHelper,
            IPayingItemHelper payingItemHelper,
            IPayingItemService payingItemService,
            ICategoryService categoryService,
            IAccountService accountService)
        {
            _payingItemHelper = payingItemHelper;
            _payingItemService = payingItemService;
            _categoryService = categoryService;
            _accountService = accountService;
            _pItemProductHelper = payingItemProductHelper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(WebUser user,int page=1)
        {
            var pItemToView = new PayingItemToView()
            {
                 PayingItems = _payingItemService.GetList()
                .Where(i=> (DateTime.Now.Date - i.Date) <= TimeSpan.FromDays(2) && i.UserId == user.Id)
                .OrderByDescending(i=>i.Date)
                .ThenBy(x=>x.Category.Name)
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList(),
                 PagingInfo = new PagingInfo()
                 {
                     CurrentPage = page,
                     ItemsPerPage = ItemsPerPage,
                     TotalItems = _payingItemService.GetList()
                     .Count(i => (DateTime.Now.Date - i.Date) <= TimeSpan.FromDays(2) && i.UserId == user.Id)
                 }
            };
            return PartialView(pItemToView);
        }

        public ActionResult ListAjax(WebUser user, int page)
        {
            var items = _payingItemService.GetList()
                .Where(i => (DateTime.Now.Date - i.Date) <= TimeSpan.FromDays(2) && i.UserId == user.Id)
                .OrderByDescending(i => i.Date)
                .ThenBy(x => x.Category.Name)
                .Skip((page - 1)*ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();
            return PartialView("PayingItemsPartial",items);
        }

        public async Task<ActionResult> Add(WebUser user, int typeOfFlow)
        {
            if (!(await _categoryService.GetListAsync()).Any(x => x.UserId == user.Id && x.Active == true && x.TypeOfFlowID == typeOfFlow))
            {
               TempData["message"] = "Сначала необходимо добавить хотя бы одну категорию";
                return PartialView("_Message");
            }
            await FillViewBag(user,typeOfFlow);
            var piModel = new PayingItemModel()
            {
                PayingItem = new PayingItem() {UserId = user.Id},
                Products = new List<Product>()
            };
            return PartialView(piModel);
        }

        [HttpPost]
        public async Task<ActionResult> Add(WebUser user,PayingItemModel pItem,int typeOfFlow)
        {
            if (ModelState.IsValid)
            {
                if (pItem.PayingItem.Date.Month > DateTime.Today.Date.Month || pItem.PayingItem.Date.Year > DateTime.Today.Year)
                {
                    pItem.PayingItem.Date = DateTime.Today.Date;
                }
                if (pItem.Products == null)
                {
                    await _payingItemService.CreateAsync(pItem.PayingItem);
                }
                else
                {
                    var summ = pItem.Products.Sum(x => x.Price);
                    if (summ != 0)
                    {
                        pItem.PayingItem.Summ = summ;
                    }
                    _payingItemHelper.CreateCommentWhileAdd(pItem);
                    await _payingItemService.CreateAsync(pItem.PayingItem);
                    await _pItemProductHelper.CreatePayingItemProduct(pItem);
                }
                return RedirectToAction("List");
            }
            else
            {
                await FillViewBag(user,typeOfFlow);
                return PartialView(pItem);
            }
        }

        public async Task<ActionResult> Edit(WebUser user, int typeOfFlowId, int id)
        {
            await FillViewBag(user, typeOfFlowId);

            var pItem = await _payingItemService.GetItemAsync(id);

            if (pItem == null)
            {
                return RedirectToAction("ListAjax");
            }

            var pItemEditModel = new PayingItemEditModel()
            {
                PayingItem = pItem,
                PayingItemProducts = new List<PaiyngItemProduct>()
            };
            PayingItemEditModel.OldCategoryId = pItem.CategoryID;

            if (!CheckForSubCategories(pItem))
            {
                return PartialView(pItemEditModel);
            }
            _pItemProductHelper.FillPayingItemEditModel(pItemEditModel, id);
            return PartialView(pItemEditModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(WebUser user,PayingItemEditModel pItem)
        {
            if (ModelState.IsValid)
            {
                if (pItem.PricesAndIdsInItem == null)
                {
                    await _payingItemService.UpdateAsync(pItem.PayingItem);
                }
                else
                {
                    pItem.PayingItem.Summ = GetSummForPayingItem(pItem);
                    _payingItemHelper.CreateCommentWhileEdit(pItem);
                    await _payingItemService.UpdateAsync(pItem.PayingItem);
                    
                    if (PayingItemEditModel.OldCategoryId != pItem.PayingItem.CategoryID)
                    {
                        await _pItemProductHelper.CreatePayingItemProduct(pItem);
                    }
                    else
                    {
                        await _pItemProductHelper.UpdatePayingItemProduct(pItem);
                    }
                }
                return RedirectToAction("List");
            }
            await FillViewBag(user, await GetTypeOfFlowId(pItem.PayingItem));
            return PartialView(pItem);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(WebUser user,int id)
        {
            await _payingItemService.DeleteAsync(id);
            return RedirectToAction("List");
        }

        public ActionResult ExpensiveCategories(WebUser user)
        {
            var tempList = _payingItemService.GetList()
                .Where(x => x.UserId == user.Id && x.Category.TypeOfFlowID == 2 && x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year)
                .ToList();

            var outList = (from item in tempList
                group item by item.Category.Name
                into grouping
                select new OverAllItem()
                {
                    Category = grouping.Key,
                    Summ = grouping.Sum(x => x.Summ)
                })
                .OrderByDescending(x=>x.Summ)
                .Take(ItemsPerPage)
                .ToList();
            
            return PartialView(outList);
        }

        public async Task<ActionResult> GetSubCategories(int id)
        {
            var products = (await _categoryService.GetItemAsync(id))
                .Product
                .OrderBy(x=>x.ProductName)
                .ToList();
            return PartialView(products);
        }

        public async Task<ActionResult> GetSubCategoriesForEdit(int id)
        {
            var products = (await _categoryService.GetProducts(id)).ToList();
            return PartialView(products);
        }

        private async Task FillViewBag(WebUser user, int typeOfFlowId)
        {
            ViewBag.Categories = (await _categoryService.GetActiveGategoriesByUser(user.Id))
                .Where(i => i.TypeOfFlowID == typeOfFlowId)
                .ToList();
            ViewBag.Accounts = (await _accountService.GetListAsync())
                .Where(x => x.UserId == user.Id)
                .ToList();
        }

        private async Task<int> GetTypeOfFlowId(PayingItem pItem)
        {
            return (await _categoryService.GetItemAsync(pItem.CategoryID)).TypeOfFlowID;
        }

        private bool CheckForSubCategories(PayingItem item)
        {
            return _payingItemService.GetList()
                .Any(x => x.CategoryID == item.CategoryID);
        }

        private decimal GetSummForPayingItem(PayingItemEditModel pItem)
        {
            if (pItem.PricesAndIdsNotInItem == null)
            {
                return pItem.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price);
            }
            else
            {
                return pItem.PricesAndIdsInItem.Where(x => x.Id != 0).Sum(x => x.Price) +
                                   pItem.PricesAndIdsNotInItem.Where(x => x.Id != 0).Sum(x => x.Price);
            }
        }

    }
}