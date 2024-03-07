using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class SystemDatum
{
    public int SystemId { get; set; }

    public string SystemName { get; set; } = null!;

    public DateTime DateStart { get; set; }

    public string SystemVersion { get; set; } = null!;

    public int? TotalUsers { get; set; }

    public bool? Licensed { get; set; }

    public DateTime DateNow { get; set; }
}
