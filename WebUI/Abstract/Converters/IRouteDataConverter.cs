using System.Collections.Generic;
using System.Web.Routing;

namespace WebUI.Abstract.Converters
{
    /// <summary>
    /// Интерфейс конвертера RouteValueDictionary в Dictionary string,object>
    /// </summary>
    public interface IRouteDataConverter
    {
        /// <summary>
        /// Конвертирует RouteValueDictionary в Dictionary string,object>
        /// </summary>
        /// <param name="routeValueDictionary"></param>
        /// <returns>Dictionary string, object></returns>
        Dictionary<string, object> ConvertRouteData(RouteValueDictionary routeValueDictionary);
    }
}