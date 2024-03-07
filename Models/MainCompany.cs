using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class MainCompany
{
    public string MainId { get; set; } = null!;

    public string MainCompanyName { get; set; } = null!;

    public DateTime DateFund { get; set; }

    public string Sector { get; set; } = null!;

    public int? TotalWorkers { get; set; }

    public string? Size { get; set; }
}
