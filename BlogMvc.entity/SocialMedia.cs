using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class SocialMedia
    {
        [Key]
        public int SocialMediaId { get; set; }
        public string SocialMediaUrl { get; set; }
        public string SocialMediaIcon { get; set; }
        public bool IsApproved { get; set; }
    }
}