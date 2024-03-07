using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public DateOnly? DateUp { get; set; }

    public bool Deprecated { get; set; }

    public string? PictureUrl { get; set; }

    public int FamilyId { get; set; }

    public int? Existances { get; set; }

    public virtual Family Family { get; set; } = null!;
}
