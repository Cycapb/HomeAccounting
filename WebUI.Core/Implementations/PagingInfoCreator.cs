using WebUI.Core.Abstract;
using WebUI.Core.Models;

namespace WebUI.Core.Implementations
{
    public class PagingInfoCreator : IPagingInfoCreator
    {
        public PagingInfo Create(int page, int itemsPerPage, int totalItems)
        {
            return new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems
            };
        }
    }
}