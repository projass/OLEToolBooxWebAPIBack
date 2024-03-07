using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
