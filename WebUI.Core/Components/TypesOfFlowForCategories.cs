using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using System;
using System.Threading.Tasks;
using WebUI.Core.Exceptions;

namespace WebUI.Core.Components
{
    public class TypesOfFlowForCategories : ViewComponent, IDisposable
    {
        private readonly ITypeOfFlowService _tofService;
        private bool _disposed = false;

        public TypesOfFlowForCategories(ITypeOfFlowService tofService)
        {
            _tofService = tofService;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var typesOfFlow = await _tofService.GetListAsync();

                return View(typesOfFlow);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(TypesOfFlowForCategories)} в методе {nameof(InvokeAsync)}", e);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposed = true;
                    _tofService.Dispose();
                }
            }
        }
    }
}
