using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using WebUI.Abstract.Converters;

namespace WebUI.Concrete.Converters
{
    public class RouteDataConverter:IRouteDataConverter
    {
        public Dictionary<string, object> ConvertRouteData(RouteValueDictionary routeValueDictionary)
        {
            return routeValueDictionary.Keys.ToDictionary(key => key, key => routeValueDictionary[key]);
        }
    }
}