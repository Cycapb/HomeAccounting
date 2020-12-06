using Microsoft.AspNetCore.Mvc.Filters;
using Services;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Filters
{
    public class UserHasCategories : IAsyncActionFilter
    {
        private readonly string _webUserKey = "WebUSer";
        private readonly ICategoryService _categoryService;
        private readonly IMessageProvider _messageProvider;

        public UserHasCategories(ICategoryService categoryService, IMessageProvider messageProvider)
        {
            _categoryService = categoryService;
            _messageProvider = messageProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = context.HttpContext.Session;
            var userHasCategories = false;
            var curUser = await session.GetJsonAsync<WebUser>(_webUserKey);
            if (curUser != null)
            {
                if (context.HttpContext.Request.RouteValues.TryGetValue("typeOfFlowId", out var value))
                {
                    var tofId = int.Parse((string)value);
                    var taskResult = await _categoryService.GetListAsync(x => x.UserId == curUser.Id && x.Active && x.TypeOfFlowID == tofId);
                    userHasCategories = taskResult.Any();
                }
                else
                {
                    var taskResult = await _categoryService.GetListAsync(x => x.UserId == curUser.Id && x.Active);
                    userHasCategories = taskResult.Any();
                }

            }
            if (!userHasCategories)
            {
                context.Result = new UserHasNoCategoriesActionResult(_messageProvider);
            }
            else
            {
                await next();
            }
        }
    }
}