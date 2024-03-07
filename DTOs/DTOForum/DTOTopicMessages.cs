using OLEToolBoxWebAPIPruebas.DTOs.DTOForum;

namespace OLEToolBoxWebAPIPruebas.DTOs.DTOForum
    {
    public class DTOTopicMessages
        {
        public int IdTopicDTO { get; set; }

        public string TitleDTO { get; set; }

        public int IdUserTopicDTO { get; set; }

        public DateTime DateCreatedDTO { get; set; }

        public List<DTOMessagesItem> MensajesDTO = new List<DTOMessagesItem>();


        }
    }
