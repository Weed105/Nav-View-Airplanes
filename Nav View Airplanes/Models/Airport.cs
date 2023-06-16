using System;
using System.Collections.Generic;

namespace Nav_View_Airplanes.Models;

public partial class Airport
{
    public int Idairport { get; set; }

    public string City { get; set; } = null!;

    public double X { get; set; }

    public double Y { get; set; }

    public virtual ICollection<Flight> FlightArrivalAirportNavigations { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightDepartureAirportNavigations { get; set; } = new List<Flight>();

    public virtual ICollection<Intermediate> Intermediates { get; set; } = new List<Intermediate>();

    public virtual ICollection<Plane> Planes { get; set; } = new List<Plane>();
}
