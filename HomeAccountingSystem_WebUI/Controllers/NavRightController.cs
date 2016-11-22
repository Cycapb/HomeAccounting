using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NavRightController : Controller
    {
        private readonly IPayingItemService _service;
        private readonly IDbHelper _dbHelper;

        public NavRightController(IPayingItemService service,IDbHelper dbHelper)
        {
            _service = service;
            _dbHelper = dbHelper;
        }

        public ActionResult MenuIncoming(WebUser user)
        {
            var collection = _service.GetListByTypeOfFlow(user,1)
                .ToList();
                
            var budgetModel = BudgetModel(collection, 1);
            return PartialView(budgetModel);
        }

        public PartialViewResult MenuOutgo(WebUser user)
        {
            var collection = _service.GetListByTypeOfFlow(user,2)
                .ToList();
            BudgetModel budgetModel = BudgetModel(collection,2);
            return PartialView(budgetModel);
        }

        private BudgetModel BudgetModel(List<PayingItem> collection, int typeOfMoney)
        {
            var budget = new BudgetModel()
            {
                TypeOfMoney = typeOfMoney == 1 ? "Доход" : "Расход",
                Month = _dbHelper.GetSummForMonth(collection),
                Week = _dbHelper.GetSummForWeek(collection),
                Day = _dbHelper.GetSummForDay(collection)
            };
            
            return budget;
        }
    }
}