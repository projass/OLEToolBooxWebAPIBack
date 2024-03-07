namespace OLEToolBoxWebAPIPruebas.DTOs.DTOUsers
    {
    public class DTOUserProfilesPut
        {

        public int IdProfileDTO { get; set; }

        public string ProfileNameDTO { get; set; }
        public string ProfileApelDTO { get; set; }
        public string? PhoneNumberDTO { get; set; }
        public string ProfileAliasDTO { get; set; }
        public IFormFile ProfileAvatarDTO { get; set; }
        public DateTime BirthdayDTO { get; set; }
        public int ProfileRoleDTO { get; set; }
        public int CredentialsIdDTO { get; set; }

        }
    }
