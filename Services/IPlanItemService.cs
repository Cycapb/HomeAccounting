using DomainModels.Model;
using Services.BaseInterfaces;
using System.Threading.Tasks;

namespace Services
{
    public interface IPlanItemService : IQueryServiceAsync<PlanItem>, ICommandServiceAsync<PlanItem>
    {
        Task SaveAsync();
    }
}