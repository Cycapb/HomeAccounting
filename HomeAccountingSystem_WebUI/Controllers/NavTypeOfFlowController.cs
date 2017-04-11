using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Services;

namespace HomeAccountingSystem_WebUI.Controllers
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
            var types = _tofService.GetList()
                .ToList();
            return PartialView(types);
        }
    }
}