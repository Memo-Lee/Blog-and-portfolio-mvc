using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class Blog
    
    { 
        [Key]
        public int BlogId { get; set; }
        public string BlogHeader { get; set; }
        public string BlogUrl { get; set; }
        public DateTime BlogDate { get; set; } //DateTime ile güncelleme
        public string BlogText { get; set; }
        public string BlogImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
    }
}