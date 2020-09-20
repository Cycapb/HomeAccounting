using WebUI.Abstract;
using WebUI.Models;

namespace WebUI.Concrete
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