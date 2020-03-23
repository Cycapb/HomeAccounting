using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebUI.Core.Infrastructure.Extensions
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.LoadAsync();
            session.Set(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            session.LoadAsync();
            var item = session.GetString(key);

            return item == null ? default : JsonConvert.DeserializeObject<T>(item);
        }
    }
}
