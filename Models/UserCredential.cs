using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class UserCredential
{
    public int CredId { get; set; }

    public string Email { get; set; } = null!;

    public string CredPassword { get; set; } = null!;

    public byte[]? Salt { get; set; }

    public string? ChangePasswordLink { get; set; }

    public DateTime? DateLink { get; set; }

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();

    public virtual ICollection<UsersDatum> UsersData { get; set; } = new List<UsersDatum>();
}
