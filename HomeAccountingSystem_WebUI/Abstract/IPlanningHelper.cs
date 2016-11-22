using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAccountingSystem_DAL.Abstract;
using HomeAccountingSystem_DAL.Model;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IPlanningHelper
    {
        Task CreatePlanItems(IWorkingUser user);
        Task CreatePlanItemsForCategory(IWorkingUser user, int categoryId);
        Task SpreadPlanItems(IWorkingUser user, PlanItem item);
        Task ActualizePlanItems(string userId);
        BalanceModel GetBalanceModel(int month, List<PlanItem> planItems);
        ViewPlaningModel GetUserBalance(IWorkingUser user, bool showAll);
    }
}
