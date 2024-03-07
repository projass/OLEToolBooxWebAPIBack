using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class Operation
{
    public int Id { get; set; }

    public DateTime DateAction { get; set; }

    public string Operation1 { get; set; } = null!;

    public string Controller { get; set; } = null!;

    public string Ip { get; set; } = null!;
}
