using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TouristoMVC.Helper;
using TouristoMVC.Models;

namespace TouristoMVC.Controllers
{
    public class MapController : Controller
    {
        private string GoogleGeocodeApiKey = "AIzaSyB4AYPMmFrJaGsxC6bwhMN8Njm-IHMglwg";
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectCountry()
        {
                ViewData["CountryList"] = GetCountriesList();
                Location location = new Location()
                {
                    Address = Request.Form["address"],
                    Langtitude = float.Parse(Request.Form["lang"]),
                    Longtitude = float.Parse(Request.Form["long"])
                };
                ViewData["Location"] = location;

            return View();
        }

        public ActionResult SelectCity(Country country)
        {
            country = GetHelper<Country>.GetById(Globals.COUNTRIES_API_LINK, country.id);
            Location location = new Location()
            {
                Address = Request.Form["address"],
                Langtitude = float.Parse(Request.Form["lang"]),
                Longtitude = float.Parse(Request.Form["long"])
            };
            ViewData["Location"] = location;
            ViewData["CityList"] = GetCityList(country);
            return View();
        }

        public ActionResult SelectAttractions(City city)
        {
            city = GetHelper<City>.GetById(Globals.CITIES_API_LINK, city.id);
            Location location = new Location()
            {
                Address = Request.Form["address"],
                Langtitude = float.Parse(Request.Form["lang"]),
                Longtitude = float.Parse(Request.Form["long"])
            };
            ViewData["Location"] = location;
            ViewData["AttractionList"] = GetAttractionList(city);
            return View();
        }

        public ActionResult SetUpStartingPoint()
        {
            var address = Request.Form["address"];
            if (string.IsNullOrEmpty(address))
            {
                address = "Zielona Góra";
            }

            Location location = new Location();
            location.Address = address;

            DataTable locationDataTable = GetLatAndLongOfStreet(getRequestUrl(location.Address));
            location.Longtitude = float.Parse(locationDataTable.Rows[0]["Longtitude"].ToString(), CultureInfo.InvariantCulture);
            location.Langtitude = float.Parse(locationDataTable.Rows[0]["Latitude"].ToString(), CultureInfo.InvariantCulture);

            return View(location);
        }

        public ActionResult FinalizeMap() 
        {
            Location location = new Location()
            {
                Address = Request.Form["address"],
                Langtitude = float.Parse(Request.Form["lang"]),
                Longtitude = float.Parse(Request.Form["long"])
            };

            ViewData["Location"] = location;
            string ids = Request.Form["id"];
            string[] attractionsIds = ids.Split(',').ToArray();
            List<Location> locations = SetLocationList(attractionsIds);
            return View(locations);
        }

        private List<Location> SetLocationList(string[] ids)
        {
            List<Location> locations = new List<Location>();
            foreach(string id in ids)
            {
                Attraction attraction = GetHelper<Attraction>.GetById(Globals.ATTRACTION_API_LINK,int.Parse(id));
                Location location = new Location();
                location.Address = attraction.city.name + " " + attraction.address;

                DataTable locationDataTable = GetLatAndLongOfStreet(getRequestUrl(location.Address));
                location.Longtitude = float.Parse(locationDataTable.Rows[0]["Longtitude"].ToString(), CultureInfo.InvariantCulture);
                location.Langtitude = float.Parse(locationDataTable.Rows[0]["Latitude"].ToString(), CultureInfo.InvariantCulture);

                locations.Add(location);
            }

            return locations;
        }

        public string getRequestUrl(string address)
        {
                address.Replace(" ", "%20");
                string url = "https://maps.google.com/maps/api/geocode/xml?address=" + address + "&sensor=false&key=" + GoogleGeocodeApiKey;
                return url;
        }

        public DataTable GetLatAndLongOfStreet(string url)
        {
            DataTable dtGMap = null;
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    DataTable dtCoordinates = new DataTable();
                    dtCoordinates.Columns.AddRange(new DataColumn[4] {
                        new DataColumn("ID",typeof(int)),
                        new DataColumn("Adress",typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longtitude",typeof(string))});
                    try
                    {
                        foreach (DataRow row in dsResult.Tables["result"].Rows)
                        {
                            string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();

                            DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];

                            dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        dtCoordinates.Rows.Add(1, "Zielona Góra", "51.9356691", "15.505642");
                    }

                    dtGMap = dtCoordinates;
                    return dtGMap;
                }
            }
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

        private IEnumerable<SelectListItem> GetCityList(Country country)
        {
            var citiesList = country.cities;

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


        private object GetAttractionList(City city)
        {
            var attractionList = city.attractions;

            var attractions = (from attraction in attractionList
                          select new SelectListItem
                          {
                              Text = attraction.name + " Type: " + attraction.type,
                              Value = attraction.id.ToString()
                          }).ToList();

            IList<SelectListItem> items = new List<SelectListItem>();

            foreach (var attraction in attractions)
            {
                items.Add(attraction);
            }

            return items;
        }
    }
}