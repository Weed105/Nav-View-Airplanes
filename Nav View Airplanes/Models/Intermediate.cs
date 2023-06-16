using System;
using System.Collections.Generic;

namespace Nav_View_Airplanes.Models;

public partial class Intermediate
{
    public int Idflight { get; set; }

    public int Idairport { get; set; }

    public virtual Airport IdairportNavigation { get; set; } = null!;

    public virtual Flight IdflightNavigation { get; set; } = null!;
}
