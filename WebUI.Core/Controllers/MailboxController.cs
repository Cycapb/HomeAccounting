using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using WebUI.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace WebUI.Core.Controllers
{
    public class MailboxController : Controller
    {
        private readonly IMailboxService _mailboxService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<MailboxController> _logger; //= Log.Logger.ForContext<MailboxController>();

        public MailboxController(
            IMailboxService mailboxService,
            ICategoryService categoryService,
            ILogger<MailboxController> logger)
        {
            _mailboxService = mailboxService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Recieving of mailboxes has started");
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                _logger.LogInformation("Recieving of mailboxes has finished");
                return Ok(mailboxes);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(MailboxController)} в методе {nameof(Index)}", e);
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