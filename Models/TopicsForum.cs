using System;
using System.Collections.Generic;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class TopicsForum
{
    public int IdTopic { get; set; }

    public int? IdUserTopic { get; set; }

    public string? Title { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual UserProfile? IdUserTopicNavigation { get; set; }

    public virtual ICollection<MessagesForum> MessagesForums { get; set; } = new List<MessagesForum>();
}
