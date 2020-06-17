using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace TouristoMVC.Helper
{
    public class GetHelper<T> where T:class
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

        public static IEnumerable<T> GetAll(string getLink)
        {

            IEnumerable<T> models = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Globals.API_LINK);
                var responseTask = client.GetAsync(getLink);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<T>>();
                    readTask.Wait();

                    models = readTask.Result;
                }
                else
                {
                    models = Enumerable.Empty<T>();
                }
            }
            return models;
        }

        public static T GetById(string getLink, int id)
        {
            T model = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Globals.API_LINK);
                var responseTask = client.GetAsync(getLink + "/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<T>();
                    readTask.Wait();

                    model = readTask.Result;
                }
            }
            return model;
        }

        public static IEnumerable<T> GetAllUsers(string getLink)
        {

            IEnumerable<T> models = null;

            using (var client = new HttpClient())
            {
                SetUserToken();
                client.BaseAddress = new Uri(Globals.API_LINK);
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                var responseTask = client.GetAsync(getLink);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<T>>();
                    readTask.Wait();

                    models = readTask.Result;
                }
                else
                {
                    models = Enumerable.Empty<T>();
                }
            }
            return models;
        }
    }
}