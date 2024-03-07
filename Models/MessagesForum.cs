using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class MessagesForum
{
    public int IdMessage { get; set; }

    public int? IdUserMessage { get; set; }

    public int? IdUserProfile { get; set; }

    public int? IdTopic { get; set; }

    public string? Text { get; set; }

    public DateTime? DateMessage { get; set; }

    public string? AliasMessage { get; set; }

    public virtual TopicsForum? IdTopicNavigation { get; set; }

    public virtual UserProfile? IdUserMessageNavigation { get; set; }
}
