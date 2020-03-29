using WebUI.Core.Models;

namespace WebUI.Core.Abstract
{
    public interface IPagingInfoCreator
    {
        PagingInfo Create(int page, int itemsPerPage, int totalItems);
    }
}
