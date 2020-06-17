using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace TouristoMVC.Helper
{
    public class PostHelper<T> where T:class
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

        public static bool PostEntity(string postLink, T entity)
        {
            using (var client = new HttpClient())
            {
                SetUserToken();
                client.BaseAddress = new Uri(Globals.API_LINK);
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                var responseTask = client.PostAsJsonAsync(postLink, entity);
                responseTask.Wait();

                var result = responseTask.Result;
                return result.IsSuccessStatusCode;
            }
        }

        public static string GetToken(string postLink, T entity)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Globals.API_LINK);

                var responseTask = client.PostAsJsonAsync(postLink, entity);
                responseTask.Wait();

                var result = responseTask.Result.Content.ReadAsStringAsync().Result.ToString();
                return result.ToString();
            }
        }
    }
}