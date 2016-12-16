using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IPlanningHelper
    {
        Task CreatePlanItems(WebUser user);
        Task CreatePlanItemsForCategory(WebUser user, int categoryId);
        Task SpreadPlanItems(WebUser user, PlanItem item);
        Task ActualizePlanItems(string userId);
        BalanceModel GetBalanceModel(int month, List<PlanItem> planItems);
        Task<ViewPlaningModel> GetUserBalance(WebUser user, bool showAll);
    }
}
