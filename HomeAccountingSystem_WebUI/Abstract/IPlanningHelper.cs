using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Model;
using WebUI.Models;

namespace WebUI.Abstract
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
