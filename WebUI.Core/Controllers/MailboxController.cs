﻿using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace WebUI.Core.Controllers
{
    public class MailboxController : Controller
    {
        private readonly IMailboxService _mailboxService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger = Log.Logger.ForContext<MailboxController>();

        public MailboxController(
            IMailboxService mailboxService,
            ICategoryService categoryService)
        {
            _mailboxService = mailboxService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //_logger.LogInformation("Receiving of mailboxes by {User}", "Anonymous");

                _logger.ForContext("RequestId", ControllerContext.HttpContext.TraceIdentifier).Information("Receiving of mailboxes by {User}", "Anonymous");
                var mailboxes = (await _mailboxService.GetListAsync()).ToList();
                //_logger.LogDebug("Test debug message");
                
                return Ok(mailboxes);
            }
            catch (ServiceException ex)
            {
                //_logger.LogError(ex, "Something went wrong");
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