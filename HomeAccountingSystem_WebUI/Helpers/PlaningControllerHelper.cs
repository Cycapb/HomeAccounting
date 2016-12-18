using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Helpers
{
    [Authorize]
    public class PlaningControllerHelper:IPlanningHelper
    {
        private readonly IPlanItemService _planItemService;
        private readonly ICategoryService _categoryService;
        private readonly IPayingItemService _pItemService;

        public PlaningControllerHelper(IPlanItemService planItemService, 
            ICategoryService categoryService, IPayingItemService pItemService)
        {
            _planItemService = planItemService;
            _categoryService = categoryService;
            _pItemService = pItemService;
        }

        public async Task CreatePlanItems(WebUser user)
        {
            var categories = (await _categoryService.GetActiveGategoriesByUser(user.Id))
                .ToList();

            for (int i = 0; i < categories.Count; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (j == 0)
                    {
                        var planItem = CreatePlanItem(user, categories[i].CategoryID, DateTime.Today);
                        await _planItemService.CreateAsync(planItem);
                    }
                    else
                    {
                        var planItem = CreatePlanItem(user, categories[i].CategoryID, DateTime.Today.AddMonths(j));
                        await _planItemService.CreateAsync(planItem);
                    }
                }
                await _planItemService.SaveAsync();
            }
        }

        public async Task CreatePlanItemsForCategory(WebUser user, int categoryId)
        {
            for (int i = 0; i < 12; i++)
            {
                if (i == 0)
                {
                    var planItem = CreatePlanItem(user, categoryId, DateTime.Today);
                    await _planItemService.CreateAsync(planItem);
                }
                else
                {
                    var planItem = CreatePlanItem(user, categoryId, DateTime.Today.AddMonths(i));
                    await _planItemService.CreateAsync(planItem);
                }
            }
            await _planItemService.SaveAsync();
        }

        public async Task SpreadPlanItems(WebUser user, PlanItem item)
        {
            var planItems = (await _planItemService.GetListAsync(user.Id))
                .Where(x => x.Closed == false 
                && x.Month.Date >= item.Month.Date
                && x.CategoryId == item.CategoryId)
                .ToList();
                foreach (var planItem in planItems)
                {
                    planItem.SummPlan = item.SummPlan;
                    await _planItemService.UpdateAsync(planItem);
                }
                await _planItemService.SaveAsync();
        }

        public async Task ActualizePlanItems(string userId)
        {
            var items = _pItemService.GetList()
                .Where(x => x.UserId == userId && x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year)
                .ToList();
            var planItems = (await _planItemService.GetListAsync(userId))
                .Where(x => x.Month.Month == DateTime.Today.Month && x.Month.Year == DateTime.Today.Year)
                .ToList();

            foreach (var planItem in planItems)
            {
                planItem.SummFact = 0;
                await _planItemService.UpdateAsync(planItem);
            }
            await _planItemService.SaveAsync();

            foreach (var payingItem in items)
            {
                foreach (var planItem in planItems)
                {
                    if (planItem.CategoryId == payingItem.CategoryID)
                    {
                        planItem.SummFact += payingItem.Summ;
                        await _planItemService.UpdateAsync(planItem);
                    }
                }
                await _planItemService.SaveAsync();
            }
        }

        public BalanceModel GetBalanceModel(int month, List<PlanItem> planItems)
        {
            planItems = planItems.Where(x => x.Month.Month == month).ToList();
            var incomeFact = planItems.First(x => x.Category?.TypeOfFlowID == 1).IncomeOutgoFact;
            var outgoFact = planItems.First(x => x.Category?.TypeOfFlowID == 2).IncomeOutgoFact;

            var incomePlan = planItems.First(x => x.Category?.TypeOfFlowID == 1).IncomePlan;
            var outgoPlan = planItems.First(x => x.Category?.TypeOfFlowID == 2).OutgoPlan;

            var balance = new BalanceModel()
            {
                BalancePlan = planItems.First().BalancePlan,
                BalanceFact = planItems.First().BalanceFact,
                OstatokFact = incomeFact - outgoFact,
                OstatokPlan = incomePlan - outgoPlan
            };
            return balance;
        }

        public async Task<ViewPlaningModel> GetUserBalance(WebUser user, bool showAll = true)
        {
            var model = new ViewPlaningModel() { Balances = new List<BalanceModel>() };
            IEnumerable<Category> categories = new List<Category>();
            if (showAll)
            {
                categories = (await _categoryService.GetActiveGategoriesByUser(user.Id)).ToList();
            }
            else
            {
                categories = (await _categoryService.GetActiveGategoriesByUser(user.Id)).Where(x => x.ViewInPlan == true).ToList();
            }
           

            model.CategoryPlanItemsIncome = categories.Where(x => x.TypeOfFlowID == 1).OrderBy(x => x.Name).ToList();
            model.CategoryPlanItemsOutgo = categories.Where(x => x.TypeOfFlowID == 2).OrderBy(x => x.Name).ToList();

            var planItems = (await _planItemService.GetListAsync(user.Id))
                .Where(x => x.Closed == false).ToList();

            model.Months = planItems
                .GroupBy(x => x.Month.ToString("Y", CultureInfo.CurrentCulture))
                .Select(x => x.Key)
                .ToList();

            model.PlanItemsIncomePlan = model.CategoryPlanItemsIncome.Any()? model.CategoryPlanItemsIncome.First().PlanItem.Where(x => x.Closed == false).ToList()
                : null;
            model.PlanItemsOutgoPlan = model.CategoryPlanItemsOutgo.Any()? model.CategoryPlanItemsOutgo.First().PlanItem.Where(x => x.Closed == false).ToList()
                : null;

            var months = planItems
                .Select(x => x.Month.Month)
                .Distinct()
                .ToList();

            foreach (var month in months)
            {
                var balance = GetBalanceModel(month, planItems);
                model.Balances.Add(balance);
            }

            return model;
        }

        private PlanItem CreatePlanItem(WebUser user,int catId, DateTime date)
        {
            return new PlanItem()
            {
                CategoryId = catId,
                Month = date,
                SummFact = 0,
                SummPlan = 0,
                IncomePlan = 0,
                OutgoPlan = 0,
                IncomeOutgoFact = 0,
                Closed = false,
                UserId = user.Id,
                BalancePlan = 0,  
                BalanceFact = 0,
            };
        }
 
    }
}
