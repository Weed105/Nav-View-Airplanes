using System;
using System.Collections.Generic;

namespace Nav_View_Airplanes.Models;

public partial class Plane
{
    public int Idplane { get; set; }

    public string? Model { get; set; }
    public int? Idairport { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public virtual Airport? IdairportNavigation { get; set; }
}
