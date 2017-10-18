using WebUI.Models;

namespace WebUI.Abstract
{
    public interface IPagingInfoCreator
    {
        PagingInfo Create(int page, int itemsPerPage, int totalItems);
    }
}
