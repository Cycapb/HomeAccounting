using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using HomeAccountingSystem_DAL.Repositories;
using HomeAccountingSystem_DAL.Model;

namespace HomeAccountingSystem_WebUI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class NavTypeOfFlowController : Controller
    {
        private readonly IRepository<TypeOfFlow> _tofRepository;

        public NavTypeOfFlowController(IRepository<TypeOfFlow> tofRepository)
        {
            _tofRepository = tofRepository;
        }
        
        public PartialViewResult List()
        {
            var types = _tofRepository.GetList()
                .ToList();
            return PartialView(types);
        }
    }
}