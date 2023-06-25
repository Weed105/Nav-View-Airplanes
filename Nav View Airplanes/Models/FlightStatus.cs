using System;
using System.Collections.Generic;

namespace Nav_View_Airplanes.Models;

public partial class FlightStatus
{
    public int IdflightStatus { get; set; }

    public string FlightStatus1 { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
