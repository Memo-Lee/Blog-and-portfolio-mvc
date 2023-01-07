using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class NoiceText
    {
        [Key]
        public int NoiceId { get; set; }
        public string NoiceContent { get; set; }
        public bool IsHome { get; set; }
    }
}