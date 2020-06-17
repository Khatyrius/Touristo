using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristoMVC.Helper;
using TouristoMVC.Models;

namespace TouristoMVC.Controllers
{
    public class AttractionsController : Controller
    {
        // GET: Attractions
        public ActionResult Index()
        {
            IEnumerable<Attraction> attractions = GetHelper<Attraction>.GetAll("attractions");
            attractions = attractions.OrderBy(a => a.id);
            if (attractions == null)
            {
                ModelState.AddModelError(string.Empty, "Server error - no data in database.");
            }

            return View(attractions);
        }

        public ViewResult New()
        {
            ViewData["CitiesList"] = GetCityList();
            return View("AddAttraction");
        }

        public ViewResult Edit(int attractionId)
        {
            ViewData["CitiesList"] = GetCityList();
            Attraction attraction = GetHelper<Attraction>.GetById(Globals.ATTRACTION_API_LINK, attractionId);
            return View("Edit", attraction);
        }

        public ActionResult DeleteAttraction(int attractionId)
        {
            DeleteHelper.DeleteEntity(Globals.ATTRACTION_API_LINK, attractionId);
            return RedirectToAction("Index");
        }

        public ActionResult AddAttraction(Attraction attraction)
        {
            City city = GetHelper<City>.GetById(Globals.CITIES_API_LINK, attraction.city.id);
            Country country = GetHelper<Country>.GetById(Globals.COUNTRIES_API_LINK, city.country.id);
            Attraction newAttraction = new Attraction()
            {
                name = attraction.name,
                address = attraction.address,
                type = attraction.type,
                city = new City
                {
                    id = city.id,
                    name = city.name,
                    country = new Country
                    {
                        id = country.id,
                        name = country.name
                    }
                }
            };

            PostHelper<Attraction>.PostEntity(Globals.ATTRACTION_API_LINK, newAttraction);

            return RedirectToAction("Index");
        }

        public ActionResult EditAttraction(Attraction attraction)
        {
            City city = GetHelper<City>.GetById(Globals.CITIES_API_LINK, attraction.city.id);
            Country country = GetHelper<Country>.GetById(Globals.COUNTRIES_API_LINK, city.country.id);
            Attraction updatedAttraction = new Attraction()
            {
                id = attraction.id,
                name = attraction.name,
                address = attraction.address,
                type = attraction.type,
                city = new City
                {
                    id = city.id,
                    name = city.name,
                    country = new Country
                    {
                        id = country.id,
                        name = country.name
                    }
                }
            };

            UpdateHelper<Attraction>.UpdateEntity(Globals.ATTRACTION_API_LINK, updatedAttraction);

            return RedirectToAction("Index");
        }

        private IEnumerable<SelectListItem> GetCityList()
        {
            var citiesList = GetHelper<City>.GetAll(Globals.CITIES_API_LINK).ToList();

            var cities = (from city in citiesList
                             select new SelectListItem
                             {
                                 Text = city.name,
                                 Value = city.id.ToString()
                             }).ToList();

            IList<SelectListItem> items = new List<SelectListItem>();

            foreach (var city in cities)
            {
                items.Add(city);
            }

            return items;
        }
    }
}