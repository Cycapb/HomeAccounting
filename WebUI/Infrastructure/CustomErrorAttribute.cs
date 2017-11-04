using System.Web.Mvc;
using Converters;
using Loggers;
using Loggers.Models;

namespace WebUI.Infrastructure
{
    public class CustomErrorAttribute : FilterAttribute, IExceptionFilter
    {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IRouteDataConverter _routeDataConverter;

        public CustomErrorAttribute(IExceptionLogger exceptionLogger, IRouteDataConverter routeDataConverter)
        {
            _exceptionLogger = exceptionLogger;
            _routeDataConverter = routeDataConverter;
        }

        public void OnException(ExceptionContext filterContext)
        {
            //ToDO Сделать определение реального IP, даже, если он за прокси
            var loggingModel = new MvcLoggingModel()
            {
                UserName = filterContext.HttpContext.User.Identity.Name,
                UserHostAddress = filterContext.HttpContext.Request.UserHostAddress,
                RouteData = _routeDataConverter.ConvertRouteData(filterContext.HttpContext.Request.RequestContext.RouteData.Values)
            };
            _exceptionLogger.LogException(filterContext.Exception, loggingModel);

            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new ViewResult()
                {
                    ViewName = "ErrorPage"
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}