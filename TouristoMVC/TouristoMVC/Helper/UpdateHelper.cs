using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace TouristoMVC.Helper
{
    public class UpdateHelper<T> where T:class
    {
        private static HttpCookie cookie;
        private static string token;

        private static void SetUserToken()
        {

            cookie = HttpContext.Current.Request.Cookies["Bearer"];
            if (cookie != null)
            {
                token = cookie.Value.ToString();
            }
            else
            {
                token = string.Empty;
            }
        }

        public static bool UpdateEntity(string updateLink, T entity)
        {
            using (var client = new HttpClient())
            {
                SetUserToken();
                client.BaseAddress = new Uri(Globals.API_LINK);
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                var responseTask = client.PutAsJsonAsync(updateLink, entity);
                responseTask.Wait();

                var result = responseTask.Result;
                return result.IsSuccessStatusCode;
            }
        }
    }
}