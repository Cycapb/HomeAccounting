using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Exceptions;
using WebUI.Core.Infrastructure.Extensions;
using WebUI.Core.Models;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Components
{
    public class ReportOverallLastYearByMonths : ViewComponent, IDisposable
    {
        private readonly IReportControllerHelper _reportControllerHelper;
        private bool _disposed = false;

        public ReportOverallLastYearByMonths(IReportControllerHelper reportControllerHelper)
        {
            _reportControllerHelper = reportControllerHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var user = await ViewComponentContext.ViewContext.HttpContext.Session.GetJsonAsync<WebUser>(nameof(WebUser));
                var model = new ReportOverallLastYearByMonthsModel();
                var payingItems = _reportControllerHelper.GetPayingItemsForLastYear(user).ToList();
                _reportControllerHelper.FillReportMonthsModel(model, payingItems);

                return View("_OverallLastYearMonths", model);
            }
            catch (WebUiHelperException e)
            {
                throw new WebUiException(
                    $"Ошибка в ViewComponent с названием {nameof(ReportOverallLastYearByMonths)} в методе {nameof(InvokeAsync)}",
                    e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reportControllerHelper.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
