using System.Collections.Generic;
using System.Web.Routing;

namespace Converters
{
    public interface IRouteDataConverter
    {
        Dictionary<string, object> ConvertRouteData(RouteValueDictionary routeValueDictionary);
    }
}