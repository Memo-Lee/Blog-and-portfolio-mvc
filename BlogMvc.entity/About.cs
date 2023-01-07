using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class About
    {
        [Key]
        public int AboutBaslikId { get; set; }
        public string AboutBaslik { get; set; }
        public string AboutText { get; set; }
        public bool IsApproved { get; set; }

    }
}