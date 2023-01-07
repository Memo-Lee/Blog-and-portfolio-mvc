using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class Career
    {
        [Key]
        public int BusinessId { get; set; }
        public string BusinessCompany { get; set; }
        public string BusinessName { get; set; }
        public string BusinessTime { get; set; }
        public bool IsApproved { get; set; }
    }
}