using System;
using System.Collections.Generic;

namespace Nav_View_Airplanes.Models;

public partial class Flight
{
    public int Idflight { get; set; }

    public int DepartureAirport { get; set; }

    public int ArrivalAirport { get; set; }

    public int Idplane { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public int Status { get; set; }

    public virtual Airport ArrivalAirportNavigation { get; set; } = null!;

    public virtual Airport DepartureAirportNavigation { get; set; } = null!;

    public virtual Plane IdplaneNavigation { get; set; } = null!;

    public virtual Intermediate? Intermediate { get; set; }

    public virtual FlightStatus StatusNavigation { get; set; } = null!;
}
