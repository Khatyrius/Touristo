using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace TouristoMVC.Helper
{
    public class DeleteHelper
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

        public static bool DeleteEntity(string deleteLink, int id)
        {
            using (var client = new HttpClient())
            {
                SetUserToken();
                client.BaseAddress = new Uri(Globals.API_LINK);
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                var responseTask = client.DeleteAsync(deleteLink + "/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                return result.IsSuccessStatusCode;
            }
        }
    }
}