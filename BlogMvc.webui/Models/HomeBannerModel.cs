using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class HomeBannerModel
    {
        public int BannerId { get; set; }
        public string BannerHeader { get; set; }
        public string BannerText { get; set; }
        public string Bannerİmage { get; set; }
        public bool IsHome { get; set; }
    }
    public class HomeBannerListViewModel
        {
            public List<HomeBanner> HomeBanners { get; set; }
            public List<SocialMedia> SocialMedias { get; set; }
            public List<ProfilePhoto> ProfilePhotos { get; set; }
            public List<Blog> Blogs { get; set; }

        }
    public class SocialMediaModel
    {
        public int SocialMediaId { get; set; }
        public string SocialMediaUrl { get; set; }
        public string SocialMediaIcon { get; set; }
        public bool IsApproved { get; set; }
    }
    public class ProfilePhotoModel
    {
        public int ProfilePhotoId { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public bool IsApproved { get; set; }
    }
    public class HomeMenuListViewModel
    {
        public List<ProfilePhoto> ProfilePhotos { get; set; } = new();
    }

}