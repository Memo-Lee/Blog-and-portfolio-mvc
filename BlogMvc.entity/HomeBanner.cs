using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMvc.entity
{
    public class HomeBanner
    {
        [Key]
        public int BannerId { get; set; }
        public string BannerHeader { get; set; }
        public string BannerText { get; set; }
        public string Bannerİmage { get; set; }
        public bool IsHome { get; set; }
    }
}