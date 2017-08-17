using System.Text;
using System.Web.Mvc;
using NLog;

namespace WebUI.Infrastructure
{
    public class CustomErrorAttribute:FilterAttribute,IExceptionFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine("\r\n");
                errorMessage.AppendLine($"Пользователь: {filterContext.HttpContext.User.Identity.Name}");
                errorMessage.AppendLine($"IP-адрес: {filterContext.HttpContext.Request.UserHostAddress}");
                errorMessage.AppendLine($"Контроллер: {filterContext.RouteData.Values["controller"]} Метод: {filterContext.RouteData.Values["action"]}");
                errorMessage.AppendLine($"Ошибка: {filterContext.Exception.Message}");
                errorMessage.AppendLine($"Трассировка стэка: {filterContext.Exception.StackTrace}");
                errorMessage.AppendLine($"InnerException: {filterContext.Exception.InnerException?.Message}");
                errorMessage.AppendLine($"InnerException StackTrace: {filterContext.Exception.InnerException?.StackTrace}");
                Logger.Error(errorMessage.ToString);

                filterContext.Result = new ViewResult()
                {
                    ViewName = "ErrorPage"
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}