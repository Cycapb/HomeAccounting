using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using WebUI.Core.Abstract.Converters;

namespace WebUI.Core.Implementations.Converters
{
    public class RouteDataConverter : IRouteDataConverter
    {
        public Dictionary<string, object> ConvertRouteData(RouteValueDictionary routeValueDictionary)
        {
            return routeValueDictionary.Keys.ToDictionary(key => key, key => routeValueDictionary[key]);
        }
    }
}