using Microsoft.AspNetCore.Mvc.Filters;
using Services;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract;
using WebUI.Core.Infrastructure.ActionResults;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Filters
{
    public class UserHasExpenseCategoriesWithPurchases : IAsyncActionFilter
    {
        private const string WebUserKey = "WebUser";

        private readonly ICategoryService _categoryService;
        private readonly IMessageProvider _messageProvider;

        public UserHasExpenseCategoriesWithPurchases(ICategoryService categoryService, IMessageProvider messageProvider)
        {
            _categoryService = categoryService;
            _messageProvider = messageProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = context.HttpContext.Session;

            if (session != null)
            {
                var user = await session.GetJsonAsync<WebUser>(WebUserKey);

                if (user != null)
                {
                    var categoriesWithPurchases = await _categoryService.GetListAsync(x => x.Active && x.UserId == user.Id && x.TypeOfFlowID == 2 && x.Products.Any());

                    if (categoriesWithPurchases.Any())
                    {
                        await next();
                    }

                    context.Result = new UserHasNoExpenseCategoriesWithPurchasesActionResult(_messageProvider);
                }
            }
        }
    }
}
