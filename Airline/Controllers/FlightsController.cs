using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Airline.Models;


namespace Airline.Controllers
{
    public class FlightsController : Controller
    {
        [HttpGet("/flights")]
        public ActionResult Index()
        {
            List<Flight> allFlights = Flight.GetAll();
            return View(allFlights);
        }

        [HttpPost("/flights")]
        public ActionResult Show(string flightNumber, DateTime departureTime, DateTime arrivalTime, string airline, string status, int departureCity, int arrivalCity)
        {
            Flight newFlight = new Flight(flightNumber, departureTime, arrivalTime, airline, status);
            newFlight.Save();
            newFlight.AddFlight(departureCity, arrivalCity);
            List<Flight> allFlights = Flight.GetAll();
            return View("Index", allFlights);
        }

        [HttpGet("/flights/new")]
        public ActionResult New()
        {
            List<City> allCities = City.GetAll();
            return View(allCities);
        }

        [HttpGet("/flights/{flightId}")]
        public ActionResult Show(int flightId)
        {
            Flight flight = Flight.Find(flightId);

            return View(flight);
        }

        [HttpPost("/flights/{flightId}/delete")]
        public ActionResult Delete(int flightId)
        {

            Flight.Delete(flightId);
            return View("Index");
        }

        [HttpGet("/flights/{flightId}/edit")]
        public ActionResult Edit(int flightId)
        {
            Flight flight = Flight.Find(flightId);

            return View(flight);
        }
        [HttpPost("/flights/{flightId}")]
        public ActionResult Edit(string flightNumber, DateTime departureTime, DateTime arrivalTime, string airline, string status, int flightId)
        {
            Flight flight = Flight.Find(flightId);
            flight.Edit(flightNumber, departureTime, arrivalTime, airline, status);

            return View("Show",flight);
        }
        // [HttpGet("/cities/{cityId}/flights/{flightId}/edit")]
        // public ActionResult Edit(int cityId, int flightId)
        // {
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     City city = City.Find(cityId);
        //     model.Add("city", city);
        //     Flight flight = Flight.Find(flightId);
        //     model.Add("flight", flight);
        //     return View(model);
        // }
        //
        // [HttpPost("/cities/{cityId}/flights/{flightId}")]
        // public ActionResult Update(int cityId, int flightId, string flightNumber, DateTime departureTime, DateTime arrivalTime, string airline, string status)
        // {
        //     Flight flight = Flight.Find(flightId);
        //     flight.Edit(flightNumber, departureTime, arrivalTime, airline, status);
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     City city = City.Find(cityId);
        //     model.Add("city", city);
        //     model.Add("flight", flight);
        //     return View("Show", model);
        // }

    }
}
