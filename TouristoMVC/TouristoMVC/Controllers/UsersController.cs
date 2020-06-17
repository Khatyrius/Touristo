using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristoMVC.Helper;
using TouristoMVC.Models;

namespace TouristoMVC.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<User> users = GetHelper<User>.GetAllUsers(Globals.USER_API_LINK);
            users = users.OrderBy(s => s.id);
            if (users == null)
            {
                ModelState.AddModelError(string.Empty, "Server error - no data in database.");
            }

            return View(users);
        }

        public ActionResult Delete(int userId)
        {
            DeleteHelper.DeleteEntity(Globals.USER_API_LINK, userId);

            return RedirectToAction("Index");
        }

        public ActionResult LoginUser()
        {
            return View("Login");
        }
        public ActionResult Login(string username, string password)
        {
            User user = new User()
            {
                username = username,
                password = password
            };

            var auth = PostHelper<User>.GetToken(Globals.USER_VALIDATE_API_LINK, user);
            if (auth != null && auth != "Wrong password or username")
            {
                HttpCookie cookie = new HttpCookie("Bearer");
                cookie.Value = auth.ToString();
                cookie.Expires = DateTime.Now.AddMinutes(15);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            HttpCookie cookie = new HttpCookie("Bearer");
            cookie.Expires = DateTime.Now.AddMinutes(-15);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }
    }
}