using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class UsersDatum
{
    public int DataId { get; set; }

    public int ProfileId { get; set; }

    public int CredId { get; set; }

    public virtual UserCredential Cred { get; set; } = null!;

    public virtual UserProfile Profile { get; set; } = null!;
}
