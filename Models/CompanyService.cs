using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class CompanyService
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? ServiceCode { get; set; }

    public bool IsOurs { get; set; }

    public bool? MainService { get; set; }

    public int? IdCompany { get; set; }

    public string ServiceDescription { get; set; } = null!;

    public bool Dependance { get; set; }

    public string? Comments { get; set; }

    public bool GeneratingCapital { get; set; }

    public double? AmountPerMonth { get; set; }
}
