using Microsoft.EntityFrameworkCore;
using Nav_View_Airplanes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Nav_View_Airplanes.Services
{
    public class GetService
    {
        public readonly FlightContext _flightContext;
        public GetService(FlightContext flightContext)
        {
            _flightContext = flightContext;
        }
        public async Task<List<Airport>> GetAirports()
        {
            List<Airport> airports = new ();
            try
            {
                var airportsDB =  _flightContext.Airports.ToList();
                await Task.Run(() =>
                {
                    airports = airportsDB;
                });
            }
            catch (Exception)
            {
                throw;
            }
            return airports;
        }        
        public async Task<List<Flight>> GetFlights()
        {
            List<Flight> flights = new ();
            try
            {
                var flightsDB =  _flightContext.Flights.ToList();
                _flightContext.Planes.ToList();
                _flightContext.FlightStatuses.ToList();
                await Task.Run(() =>
                {
                    flights = flightsDB;
                });
            }
            catch (Exception)
            {
                throw;
            }
            return flights;
        }
        public async Task<List<Plane>> GetPlanes()
        {
            List<Plane> planes = new ();
            try
            {
                var planesDB =  _flightContext.Planes.ToList();
                await Task.Run(() =>
                {
                    planes = planesDB;
                });
            }
            catch (Exception)
            {
                throw;
            }
            return planes;
        }
        public async Task<List<User>> GetUsers()
        {
            List<User> users = new ();
            try
            {
                var usersDB =  _flightContext.Users.ToList();
                await Task.Run(() =>
                {
                    users = usersDB;
                });
            }
            catch (Exception)
            {
                throw;
            }
            return users;
        }
        public async void AddPlane(Plane plane)
        {
            _flightContext.Planes.Add(plane);
            _flightContext.SaveChanges();
        }
        public async void ChangePlane(Plane plane)
        {
            Plane plane1 = _flightContext.Planes.Where(i => i.Idplane == plane.Idplane).FirstOrDefault();
            plane1.Model = plane.Model;
            plane1.Idairport = plane.Idairport;
            _flightContext.SaveChanges();
        }
        public async void DeletePlane(Plane plane)
        {
            _flightContext.Planes.Remove(plane);
            _flightContext.SaveChanges();
        }
        public async void AddUser(User user)
        {
            _flightContext.Users.Add(user);
            _flightContext.SaveChanges();
        }
        public async void ChangeUser(User user)
        {
            User user1 = _flightContext.Users.Where(i => i.Iduser == user.Iduser).FirstOrDefault();
            user1.Surname = user.Surname;
            user1.Name = user.Name;
            user1.Patronymic = user.Patronymic;
            user1.Login = user.Login;
            user1.Password = user.Password;
            _flightContext.SaveChanges();
        }
        public async void DeleteUser(User user)
        {
            _flightContext.Users.Remove(user);
            _flightContext.SaveChanges();
        }
        public async void AddFlight(Flight flight)
        {
            _flightContext.Flights.Add(flight);
            _flightContext.SaveChanges();
        }
        public async void ChangeFlight(Flight flight)
        {
            Flight flight1 = _flightContext.Flights.Where(i => i.Idflight == flight.Idflight).FirstOrDefault();
            flight1.ArrivalTime = flight.ArrivalTime;
            flight1.ArrivalAirport = flight.ArrivalAirport;
            flight1.DepartureAirport = flight.DepartureAirport;
            flight1.DepartureTime = flight.DepartureTime;
            flight1.Idflight = flight.Idflight;
            flight1.Status = flight.Status;
            _flightContext.SaveChanges();
        }
        public async void DeleteFlight(Flight flight)
        {
            _flightContext.Flights.Remove(flight);
            _flightContext.SaveChanges();
        }
        public async void ChangeState(Flight flight)
        {
            Flight flight1 = _flightContext.Flights.FirstOrDefault(i => i.Idflight == flight.Idflight);
            flight1.Status= flight.Status;
            _flightContext.SaveChanges();
        }
    }
}
