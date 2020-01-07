using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Services;
using Services.Exceptions;
using WebUI.Exceptions;

namespace WebUI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NavTypeOfFlowController : Controller
    {
        private readonly ITypeOfFlowService _tofService;

        public NavTypeOfFlowController(ITypeOfFlowService tofService)
        {
            _tofService = tofService;
        }
        
        public PartialViewResult List()
        {
            try
            {
                var types = _tofService.GetList()
                    .ToList();
                return PartialView(types);
            }
            catch (ServiceException e)
            {
                throw new WebUiException($"Ошибка в контроллере {nameof(NavTypeOfFlowController)} в методе {nameof(List)}", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _tofService.Dispose();

            base.Dispose(disposing);
        }
    }
}