using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Airline;

namespace Airline.Models
{
    public class City
    {
        private string _name;
        private int _id;

        public City(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }


        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public void AddFlight(Flight newFlight)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO airport (flight_id, departure_city_id, arrival_city_id) VALUES (@FlightId, @DepartureCityId, @ArrivalCityId);";

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = newFlight.GetId();
            cmd.Parameters.Add(flight_id);

            MySqlParameter departure_city_id = new MySqlParameter();
            departure_city_id.ParameterName = "@DepartureCityId";
            departure_city_id.Value = newFlight.GetDepartureCity();
            cmd.Parameters.Add(departure_city_id);

            MySqlParameter arrival_city_id = new MySqlParameter();
            arrival_city_id.ParameterName = "@ArrivalCityId";
            arrival_city_id.Value = newFlight.GetArrivalCity();
            cmd.Parameters.Add(arrival_city_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<City> GetAll()
        {
            List<City> allCities = new List<City> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int CityId = rdr.GetInt32(1);
                string CityName = rdr.GetString(0);
                City newCity = new City(CityName, CityId);
                allCities.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }

        public static City Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int CityId = 0;
            string CityName = "";
            while (rdr.Read())
            {
                CityId = rdr.GetInt32(1);
                CityName = rdr.GetString(0);
            }
            City newCity = new City(CityName, CityId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newCity;
        }

        public List<Flight> GetDepartureFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
            JOIN airport ON (cities.id = airport.departure_city_id)
            JOIN flights ON (airport.flight_id = flights.id)
            WHERE cities.id = @DepartureCityId;";
            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@DepartureCityId";
            cityIdParameter.Value = _id;
            cmd.Parameters.Add(cityIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> flights = new List<Flight> { };
            while (rdr.Read())
            {
                string flightNumber = rdr.GetString(0);
                DateTime flightDepartureTime = rdr.GetDateTime(1);
                DateTime flightArrivalTime = rdr.GetDateTime(2);
                string flightAirline = rdr.GetString(3);
                string flightStatus = rdr.GetString(4);
                int flightId = rdr.GetInt32(5);
                Flight newFlight = new Flight(flightNumber, flightDepartureTime, flightArrivalTime, flightAirline, flightStatus, flightId);
                flights.Add(newFlight);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return flights;
        }

        public List<Flight> GetArrivalFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
            JOIN airport ON (cities.id = airport.arrival_city_id)
            JOIN flights ON (airport.flight_id = flights.id)
            WHERE cities.id = @ArrivalCityId;";
            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@ArrivalCityId";
            cityIdParameter.Value = _id;
            cmd.Parameters.Add(cityIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> flights = new List<Flight> { };
            while (rdr.Read())
            {
                string flightNumber = rdr.GetString(0);
                DateTime flightDepartureTime = rdr.GetDateTime(1);
                DateTime flightArrivalTime = rdr.GetDateTime(2);
                string flightAirline = rdr.GetString(3);
                string flightStatus = rdr.GetString(4);
                int flightId = rdr.GetInt32(5);
                Flight newFlight = new Flight(flightNumber, flightDepartureTime, flightArrivalTime, flightAirline, flightStatus, flightId);
                flights.Add(newFlight);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return flights;
        }

        public override bool Equals(System.Object otherCity)
        {
            if (!(otherCity is City))
            {
                return false;
            }
            else
            {
                City newCity = (City)otherCity;
                bool idEquality = this.GetId().Equals(newCity.GetId());
                bool nameEquality = this.GetName().Equals(newCity.GetName());
                return (idEquality && nameEquality);
            }
        }
        public override int GetHashCode()
        {
            string combinedHash = this.GetId() + this.GetName();
            return combinedHash.GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM cities WHERE id = @CityId; DELETE FROM airport WHERE city_id = @CityId;", conn);
            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@CityId";
            cityIdParameter.Value = this.GetId();
            cmd.Parameters.Add(cityIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
}
