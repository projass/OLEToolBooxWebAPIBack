using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class UserProfile
{
    public int ProfileId { get; set; }

    public string ProfileName { get; set; } = null!;

    public string ProfileApel { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string ProfileAlias { get; set; } = null!;

    public string? ProfileAvatar { get; set; }

    public int ProfileRole { get; set; }

    public DateTime Birthday { get; set; }

    public int CredentialsId { get; set; }

    public virtual UserCredential Credentials { get; set; } = null!;

    public virtual ICollection<MessagesForum> MessagesForums { get; set; } = new List<MessagesForum>();

    public virtual Role ProfileRoleNavigation { get; set; } = null!;

    public virtual ICollection<TopicsForum> TopicsForums { get; set; } = new List<TopicsForum>();

    public virtual ICollection<UsersDatum> UsersData { get; set; } = new List<UsersDatum>();
}
