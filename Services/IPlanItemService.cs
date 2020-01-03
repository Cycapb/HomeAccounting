using DomainModels.Model;
using Services.BaseInterfaces;
using System.Threading.Tasks;

namespace Services
{
    public interface IPlanItemService : IQueryServiceAsync<PlanItem>
    {
        Task CreateAsync(PlanItem planItem);
        Task UpdateAsync(PlanItem planItem);
        Task SaveAsync();
    }
}