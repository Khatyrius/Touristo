using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristoMVC.Helper;
using TouristoMVC.Models;

namespace TouristoMVC.Controllers
{
    public class CitiesController : Controller
    {
        // GET: Cities
        public ActionResult Index()
        {
            IEnumerable<City> cities = GetHelper<City>.GetAll("cities");
            cities = cities.OrderBy(c => c.id);
            if (cities == null)
            {
                ModelState.AddModelError(string.Empty, "Server error - no data in database.");
            }

            return View(cities);
        }

        public ViewResult New()
        {
            ViewData["CountriesList"] = GetCountriesList();
            return View("AddCity");
        }

        public ViewResult Edit(int cityId)
        {
            ViewData["CountriesList"] = GetCountriesList();
            City city = GetHelper<City>.GetById(Globals.CITIES_API_LINK, cityId);
            return View("Edit", city);
        }

        public ActionResult DeleteCity(int cityId)
        {
            DeleteHelper.DeleteEntity(Globals.CITIES_API_LINK, cityId);
            return RedirectToAction("Index");
        }

        public ActionResult AddCity(City city)
        {
            Country country = GetHelper<Country>.GetById(Globals.COUNTRIES_API_LINK, city.country.id);
            city.country = country;

            City newCity = new City
            {
                name = city.name,
                country = new Country
                {
                    id = country.id,
                    name = country.name
                }
            };

            PostHelper<City>.PostEntity(Globals.CITIES_API_LINK, newCity);

            return RedirectToAction("Index");
        }

        public ActionResult EditCity(City city)
        {
            Country country = GetHelper<Country>.GetById(Globals.COUNTRIES_API_LINK, city.country.id);
            city.country = country;

            City updatedCity = new City
            {
                id = city.id,
                name = city.name,
                country = new Country
                {
                    id = country.id,
                    name = country.name
                }
            };

            UpdateHelper<City>.UpdateEntity(Globals.CITIES_API_LINK, updatedCity);

            return RedirectToAction("Index");
        }

        public ActionResult ViewAttractions(int cityId)
        {
            City cityGet = GetHelper<City>.GetById(Globals.CITIES_API_LINK, cityId);
            return View("ViewAttractions", cityGet.attractions);
        }

        private IEnumerable<SelectListItem> GetCountriesList()
        {
            var countriesList = GetHelper<Country>.GetAll(Globals.COUNTRIES_API_LINK).ToList();

            var countries = (from country in countriesList
                             select new SelectListItem
                                {
                                    Text = country.name,
                                    Value = country.id.ToString()
                                }).ToList();

            IList<SelectListItem> items = new List<SelectListItem>();

            foreach (var country in countries)
            {
                items.Add(country);
            }

            return items;
        }
    }
}