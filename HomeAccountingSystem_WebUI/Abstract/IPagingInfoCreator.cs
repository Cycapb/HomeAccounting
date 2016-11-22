using HomeAccountingSystem_WebUI.Models;

namespace HomeAccountingSystem_WebUI.Abstract
{
    public interface IPagingInfoCreator
    {
        PagingInfo Create(int page, int itemsPerPage, int totalItems);
    }
}
