using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace WebUI.Core.Abstract.Converters
{
    /// <summary>
    /// Converts RouteValueDictionary to Dictionary string,object>
    /// </summary>
    public interface IRouteDataConverter
    {
        /// <summary>
        /// Converts RouteValueDictionary to Dictionary string,object>
        /// </summary>
        /// <param name="routeValueDictionary"></param>
        /// <returns>Dictionary string, object></returns>
        Dictionary<string, object> ConvertRouteData(RouteValueDictionary routeValueDictionary);
    }
}