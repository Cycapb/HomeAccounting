using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebUI.Core.Abstract;
using WebUI.Core.Exceptions;
using WebUI.Core.Models;
using WebUI.Core.Models.PayingItemModels;

namespace WebUI.Core.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PayingItemsController : Controller
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IPayingItemCreator _payingItemCreator;
        private readonly IPayingItemEditViewModelCreator _payingItemEditViewModelCreator;
        public int ItemsPerPage = 10;

        public PayingItemsController(IPayingItemService payingItemService, IPayingItemCreator payingItemCreator, IPayingItemEditViewModelCreator payingItemEditViewModelCreator)
        {
            _payingItemService = payingItemService;
            _payingItemCreator = payingItemCreator;
            _payingItemEditViewModelCreator = payingItemEditViewModelCreator;
        }

        [HttpGet("{userId}")]
        [Produces("application/xml", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PayingItemsList(string userId, [FromQuery] int page = 1)
        {
            try
            {
                var dateToday = DateTime.Now.Date;
                var dateMinusTwoDays = DateTime.Now.Date - TimeSpan.FromDays(2);
                var taskResult = await _payingItemService.GetListAsync(i => i.UserId == userId && (i.Date >= dateMinusTwoDays && i.Date <= dateToday));
                var payingItems = taskResult.ToList();
                var payingItemsToView = new PayingItemsListWithPaginationModel()
                {
                    PayingItems = payingItems
                        .OrderByDescending(i => i.Date)
                        .ThenBy(x => x.Category.Name)
                        .Skip((page - 1) * ItemsPerPage)
                        .Take(ItemsPerPage),
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = page,
                        ItemsPerPage = ItemsPerPage,
                        TotalItems = payingItems.Count
                    }
                };

                return Ok(payingItemsToView);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(PayingItemsList)}",
                    e);
            }
            catch (Exception e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(PayingItemsList)}",
                    e);
            }
        }

        [HttpGet("{userId}/{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPayingItem(string userId, int id)
        {
            try
            {
                var payingItemEditModel = await _payingItemEditViewModelCreator.CreateViewModel(id);

                if (payingItemEditModel == null)
                {
                    return NotFound();
                }

                return Ok(payingItemEditModel);
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetPayingItem)}", e);
            }
            catch (WebUiException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetPayingItem)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(GetPayingItem)}", e);
            }
        }

        [HttpPost("{userId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(string userId, [FromBody] PayingItemModel model)
        {
            try
            {
                await _payingItemCreator.CreatePayingItemFromViewModel(model);

                return CreatedAtAction(nameof(Add), model.PayingItem.ItemID);
            }
            catch (WebUiException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Add)}", e);
            }
            catch (Exception e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Add)}", e);
            }
        }

        [HttpDelete("{userId}/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string userId, int id)
        {
            try
            {
                await _payingItemService.DeleteAsync(id);

                return NoContent();
            }
            catch (ServiceException e)
            {
                throw new WebUiException(
                    $"Ошибка в контроллере {nameof(PayingItemController)} в методе {nameof(Delete)}", e);
            }
        }
    }
}
