using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MailboxController> _logger;

        public MailboxController(IMailboxService mailboxService, ICategoryService categoryService, ILogger<MailboxController> logger)
        {
            _mailboxService = mailboxService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogDebug("Receiving of mailboxes");
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                return Ok(mailboxes);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return NotFound();
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