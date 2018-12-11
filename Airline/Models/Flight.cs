using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Airline;

namespace Airline.Models
{
    public class Flight
    {
        private string _flightNumber;
        private DateTime _departureTime;
        private DateTime _arrivalTime;
        private string _airline;
        private string _status;
        private int _id;


        public Flight(string flightNumber, DateTime departureTime, DateTime arrivalTime, string airline, string status, int id = 0)
        {
            _flightNumber = flightNumber;
            _departureTime = departureTime;
            _arrivalTime = arrivalTime;
            _airline = airline;
            _status = status;
            _id = id;
        }
        public string GetFlightNumber()
        {
            return _flightNumber;
        }
        public void SetFlightNumber(string flightNumber)
        {
            _flightNumber = flightNumber;
        }
        public DateTime GetDepartureTime()
        {
            return _departureTime;
        }
        public void SetDepartureTime(DateTime departureTime)
        {
            _departureTime = departureTime;
        }

        public DateTime GetArrivalTime()
        {
            return _arrivalTime;
        }
        public void SetArrivalTime(DateTime arrivalTime)
        {
            _arrivalTime = arrivalTime;
        }
        public string GetAirline()
        {
            return _airline;
        }
        public string GetStatus()
        {
            return _status;
        }
        public int GetId()
        {
            return _id;
        }

        public void AddFlight(int DepartureCityId, int ArrivalCityId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO airport (flight_id, departure_city_id, arrival_city_id) VALUES (@FlightId, @DepartureCityId, @ArrivalCityId);";

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = _id;
            cmd.Parameters.Add(flight_id);

            MySqlParameter departure_city_id = new MySqlParameter();
            departure_city_id.ParameterName = "@DepartureCityId";
            departure_city_id.Value = DepartureCityId;
            cmd.Parameters.Add(departure_city_id);

            MySqlParameter arrival_city_id = new MySqlParameter();
            arrival_city_id.ParameterName = "@ArrivalCityId";
            arrival_city_id.Value = ArrivalCityId;
            cmd.Parameters.Add(arrival_city_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                string flightNumber = rdr.GetString(0);
                DateTime flightDepartureTime = rdr.GetDateTime(1);
                DateTime flightArrivalTime = rdr.GetDateTime(2);
                string flightAirline = rdr.GetString(3);
                string flightStatus = rdr.GetString(4);
                int id = rdr.GetInt32(5);
                Flight newFlight = new Flight(flightNumber, flightDepartureTime, flightArrivalTime, flightAirline, flightStatus, id);
                allFlights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allFlights;
        }

        public static void Delete(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights WHERE id = @FlightId;";
            MySqlParameter flightdParameter = new MySqlParameter();
            flightdParameter.ParameterName = "@FlightId";
            flightdParameter.Value = id;
            cmd.Parameters.Add(flightdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            string flightNumber = "";
            DateTime flightDepartureTime = new DateTime();
            DateTime flightArrivalTime = new DateTime();
            string flightAirline = "";
            string flightStatus = "";
            int flightId = 0;


            while (rdr.Read())
            {
                flightNumber = rdr.GetString(0);
                flightDepartureTime = rdr.GetDateTime(1);
                flightArrivalTime = rdr.GetDateTime(2);
                flightAirline = rdr.GetString(3);
                flightStatus = rdr.GetString(4);
                flightId = rdr.GetInt32(5);
            }
            Flight foundFlight = new Flight(flightNumber, flightDepartureTime, flightArrivalTime, flightAirline, flightStatus, flightId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundFlight;
        }
        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight newFlight = (Flight)otherFlight;
                bool flightNumberEquality = this.GetFlightNumber() == newFlight.GetFlightNumber();
                bool departureTimeEquality = this.GetDepartureTime() == newFlight.GetDepartureTime();
                bool arrivalTimeEquality = this.GetArrivalTime() == newFlight.GetArrivalTime();
                bool airlineEquality = this.GetAirline() == newFlight.GetAirline();
                bool statusEquality = this.GetStatus() == newFlight.GetStatus();
                return (flightNumberEquality && departureTimeEquality && arrivalTimeEquality && airlineEquality && statusEquality);
            }
        }
        public override int GetHashCode()
        {
            string combinedHash = this.GetFlightNumber() + this.GetDepartureTime() + this.GetArrivalTime() + this.GetAirline() + this.GetStatus();
            return combinedHash.GetHashCode();
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (flight_number,departure_time,arrival_time,airline,status) VALUES (@flightNumber,@departureTime,@arrivalTime,@airline,@status);";
            MySqlParameter flight_number = new MySqlParameter();
            flight_number.ParameterName = "@flightNumber";
            flight_number.Value = this._flightNumber;
            cmd.Parameters.Add(flight_number);

            MySqlParameter departure_time = new MySqlParameter();
            departure_time.ParameterName = "@departureTime";
            departure_time.Value = this._departureTime;
            cmd.Parameters.Add(departure_time);

            MySqlParameter arrival_time = new MySqlParameter();
            arrival_time.ParameterName = "@arrivalTime";
            arrival_time.Value = this._arrivalTime;
            cmd.Parameters.Add(arrival_time);

            MySqlParameter airline = new MySqlParameter();
            airline.ParameterName = "@airline";
            airline.Value = this._airline;
            cmd.Parameters.Add(airline);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@status";
            status.Value = this._status;
            cmd.Parameters.Add(status);

            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public List<City> GetDepartureCity()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT departure_city_id FROM airport WHERE flight_id = @flightId;";
            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@flightId";
            flightIdParameter.Value = _id;
            cmd.Parameters.Add(flightIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<int> departureCityIds = new List<int> { };
            while (rdr.Read())
            {
                int departureCityId = rdr.GetInt32(0);
                departureCityIds.Add(departureCityId);
            }
            rdr.Dispose();
            List<City> cities = new List<City> { };
            foreach (int departureCityId in departureCityIds)
            {
                var cityQuery = conn.CreateCommand() as MySqlCommand;
                cityQuery.CommandText = @"SELECT * FROM cities WHERE id = @departureCityId;";
                MySqlParameter departureCityIdParameter = new MySqlParameter();
                departureCityIdParameter.ParameterName = "@departureCityId";
                departureCityIdParameter.Value = departureCityId;
                cityQuery.Parameters.Add(departureCityIdParameter);
                var cityQueryRdr = cityQuery.ExecuteReader() as MySqlDataReader;
                while (cityQueryRdr.Read())
                {
                    int thisdepartureCityId = cityQueryRdr.GetInt32(1);
                    string cityName = cityQueryRdr.GetString(0);
                    City foundcity = new City(cityName, thisdepartureCityId);
                    cities.Add(foundcity);
                }
                cityQueryRdr.Dispose();
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return cities;
        }

        public List<City> GetArrivalCity()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT arrival_city_id FROM airport WHERE flight_id = @flightId;";
            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@flightId";
            flightIdParameter.Value = _id;
            cmd.Parameters.Add(flightIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<int> arrivalCityIds = new List<int> { };
            while (rdr.Read())
            {
                int arrivalCityId = rdr.GetInt32(0);
                arrivalCityIds.Add(arrivalCityId);
            }
            rdr.Dispose();
            List<City> cities = new List<City> { };
            foreach (int arrivalCityId in arrivalCityIds)
            {
                var cityQuery = conn.CreateCommand() as MySqlCommand;
                cityQuery.CommandText = @"SELECT * FROM cities WHERE id = @arrivalCityId;";
                MySqlParameter arrivalCityIdParameter = new MySqlParameter();
                arrivalCityIdParameter.ParameterName = "@arrivalCityId";
                arrivalCityIdParameter.Value = arrivalCityId;
                cityQuery.Parameters.Add(arrivalCityIdParameter);
                var cityQueryRdr = cityQuery.ExecuteReader() as MySqlDataReader;
                while (cityQueryRdr.Read())
                {
                    int thisarrivalCityId = cityQueryRdr.GetInt32(1);
                    string cityName = cityQueryRdr.GetString(0);
                    City foundcity = new City(cityName, thisarrivalCityId);
                    cities.Add(foundcity);
                }
                cityQueryRdr.Dispose();
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return cities;
        }
        public void AddDepartureCity(City newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO airport (departure_city_id, flight_id) VALUES (@departureCity, @flightId);";
            MySqlParameter departure_city_id = new MySqlParameter();
            departure_city_id.ParameterName = "@departureCity";
            departure_city_id.Value = newCity.GetId();
            cmd.Parameters.Add(departure_city_id);
            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@flightId";
            flight_id.Value = _id;
            cmd.Parameters.Add(flight_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Edit(string newFlightNumber, DateTime newDepartureTime, DateTime newArrivalTime, string newAirline, string newStatus)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE flights SET flight_number = @newFlightNumber, departure_time = @newDepartureTime, arrival_time = @newArrivalTime, airline = @newAirline, status = @newStatus   WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter flight_number = new MySqlParameter();
            flight_number.ParameterName = "@newFlightNumber";
            flight_number.Value = newFlightNumber;
            cmd.Parameters.Add(flight_number);

            MySqlParameter departure_time = new MySqlParameter();
            departure_time.ParameterName = "@newDepartureTime";
            departure_time.Value = newDepartureTime;
            cmd.Parameters.Add(departure_time);

            MySqlParameter arrival_time = new MySqlParameter();
            arrival_time.ParameterName = "@newArrivalTime";
            arrival_time.Value = newArrivalTime;
            cmd.Parameters.Add(arrival_time);

            MySqlParameter airline = new MySqlParameter();
            airline.ParameterName = "@newAirline";
            airline.Value = newAirline;
            cmd.Parameters.Add(airline);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@newStatus";
            status.Value = newStatus;
            cmd.Parameters.Add(status);

            cmd.ExecuteNonQuery();
            _flightNumber = newFlightNumber;
            _departureTime = newDepartureTime;
            _arrivalTime = newArrivalTime;
            _airline = newAirline;
            _status = newStatus;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
