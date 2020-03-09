using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Core.Controllers
{
    public class MailboxController : Controller
    {
        private readonly IMailboxService _mailboxService;
        private readonly ICategoryService _categoryService;

        public MailboxController(IMailboxService mailboxService, ICategoryService categoryService)
        {
            _mailboxService = mailboxService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                return Ok(mailboxes);
            }
            catch (ServiceException)
            {
                throw; //new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Index)}", e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(int id)
        {

            try
            {
                var item = await _categoryService.GetItemAsync(id);

                var outputItems = item.PayingItems.Select(x => 
                new { PayingItem_Id = x.ItemID, Account = x.Account.AccountName, Category = x.Category.Name, Products = x.PayingItemProducts.Select(x => x.Product.ProductName) });

                return Ok(outputItems);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}