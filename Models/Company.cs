using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompanyComCode { get; set; }

    public bool ReceivingServices { get; set; }

    public bool IsClient { get; set; }

    public string? CompanyInfo { get; set; }

    public DateTime? DateFound { get; set; }

    public bool IsActive { get; set; }

    public int Debt { get; set; }

    public double? DataOweAlien { get; set; }

    public double? DataOweOwn { get; set; }

    public string? Comments { get; set; }
}
