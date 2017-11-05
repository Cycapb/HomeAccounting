using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Converters;

namespace BussinessLogic.Converters
{
    public class RouteDataConverter:IRouteDataConverter
    {
        public Dictionary<string, object> ConvertRouteData(RouteValueDictionary routeValueDictionary)
        {
            return routeValueDictionary.Keys.ToDictionary(key => key, key => routeValueDictionary[key]);
        }
    }
}