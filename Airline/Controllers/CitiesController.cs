using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Airline.Models;

namespace Airline.Controllers
{
    public class CitiesController : Controller
    {

        [HttpGet("/cities")]
        public ActionResult Index()
        {
            List<City> allCities = City.GetAll();
            return View(allCities);
        }

        [HttpGet("/cities/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/cities")]
        public ActionResult Create(string cityName)
        {
            City newCity = new City(cityName);
            newCity.Save();
            List<City> allCities = City.GetAll();
            return View("Index", allCities);
        }

        [HttpGet("/cities/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            City selectedCity = City.Find(id);
            List<Flight> cityDepartureFlights = selectedCity.GetDepartureFlights();
            List<Flight> cityArrivalFlights = selectedCity.GetArrivalFlights();

            model.Add("departureFlights", cityDepartureFlights);
            model.Add("arrivalFlights", cityArrivalFlights);
            return View(model);
        }

        // This one creates new Flights within a given City, not new Cities:
        // [HttpPost("/cities/{cityId}/flights")]
        // public ActionResult Create(int cityId, string flightNumber, DateTime departureTime, DateTime arrivalTime, string airline, string status)
        // {
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     City foundCity = City.Find(cityId);
        //     Flight newFlight = new Flight(flightNumber, departureTime, arrivalTime, airline, status);
        //     newFlight.Save();
        //     // foundCity.AddFlight(newFlight);
        //     List<Flight> cityFlights = foundCity.GetDepartureFlights();
        //     model.Add("flights", cityFlights);
        //     model.Add("city", foundCity);
        //     return View("Show", model);
        // }
    }
}
