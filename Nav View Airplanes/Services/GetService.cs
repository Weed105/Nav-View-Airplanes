using Microsoft.EntityFrameworkCore;
using Nav_View_Airplanes.Models;
using System;
using System.Collections.Generic;
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
                var airportsDB = _flightContext.Airports.ToList();
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
    }
}
