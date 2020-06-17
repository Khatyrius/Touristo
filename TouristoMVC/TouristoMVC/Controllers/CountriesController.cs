using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristoMVC.Helper;
using TouristoMVC.Models;

namespace TouristoMVC.Controllers
{
    public class CountriesController : Controller
    {
        // GET: Countries
        public ActionResult Index()
        {
            IEnumerable<Country> countries = GetHelper<Country>.GetAll("countries");
            countries = countries.OrderBy(c => c.id);
            if (countries == null)
            {
                ModelState.AddModelError(string.Empty, "Server error - no data in database.");
            }

            return View(countries);
        }

        public ViewResult New()
        {
            return View("AddCountry");
        }

        public ViewResult Edit(int countryId)
        {
            Country country = GetHelper<Country>.GetById(Globals.COUNTRIES_API_LINK, countryId);
            return View("Edit", country);
        }

        public ActionResult DeleteCountry(int countryID)
        {
            DeleteHelper.DeleteEntity(Globals.COUNTRIES_API_LINK, countryID);
            return RedirectToAction("Index");
        }

        public ActionResult AddCountry(Country country)
        {
            PostHelper<Country>.PostEntity(Globals.COUNTRIES_API_LINK, country);

            return RedirectToAction("Index");
        }

        public ActionResult EditCountry(Country country)
        {
            UpdateHelper<Country>.UpdateEntity(Globals.COUNTRIES_API_LINK, country);

            return RedirectToAction("Index");
        }

        public ActionResult ViewCities(int countryId)
        {
            Country countryGet = GetHelper<Country>.GetById("countries", countryId);
            return View("ViewCities", countryGet.cities);
        }
    }
}