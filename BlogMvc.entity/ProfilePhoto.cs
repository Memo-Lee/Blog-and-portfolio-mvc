using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class ProfilePhoto
    {
        [Key]
        public int ProfilePhotoId { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public bool IsApproved { get; set; }
    }
}