using HomeAccountingSystem_WebUI.Abstract;
using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Concrete
{
    public class PagingInfoCreator:IPagingInfoCreator
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