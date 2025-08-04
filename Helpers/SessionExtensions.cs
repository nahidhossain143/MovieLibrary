using Newtonsoft.Json;
using System.Web;

namespace MovieLibrary.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject(this HttpSessionStateBase session, string key, object value)
        {
            session[key] = JsonConvert.SerializeObject(value);
        }

        public static T GetObject<T>(this HttpSessionStateBase session, string key)
        {
            var value = session[key] as string;
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
