using System;
namespace adaptatechwebapibackend.DTOs.MensajesForo
    {
    public class DTOMessagesForum
        {
        public int? IdMessageDTO { get; set; }
        public int? IdUserProfileDTO { get; set; }
        public int? IdTopicDTO { get; set; }
        public string TextDTO { get; set; }

        public string? AliasMessageDTO { get; set; }
        public DateTime? DateMessageDTO { get; set; }
        }
    }

