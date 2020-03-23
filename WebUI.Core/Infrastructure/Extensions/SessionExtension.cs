using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebUI.Core.Infrastructure.Extensions
{
    public static class SessionExtension
    {
        public static async Task SetJsonAsync<T>(this ISession session, string key, T value)
        {
            await session.LoadAsync();
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static async Task<T> GetJsonAsync<T>(this ISession session, string key)
        {
            await session.LoadAsync();
            var item = session.GetString(key);

            return item == null ? default : JsonConvert.DeserializeObject<T>(item);
        }
    }
}
