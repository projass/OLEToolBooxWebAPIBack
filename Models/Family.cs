using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class Family
{
    public int IdFamily { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
