using System;
using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class ProjectViewModel
    {
        public class PageInfo
        {
            public int TotalItems { get; set; }
            // tüm itemler
            public int ItemsPerPage { get; set; }
            // Sayfa Başına Öğeler 
            public int CurrentPage { get; set; }
            // Geçerli sayfa 
            public string CurrentCategory { get; set; }
            // Mevcut kategori
            public int TotalPages()
            // Toplam sayfalar
            {
                return (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage);
            }
        }
        public class ProjectListViewModel
        {
            public PageInfo PageInfo { get; set; }
            public List<Project> Projects { get; set; }
            public List<SocialMedia> SocialMedias { get; set; }
            public List<ProfilePhoto> ProfilePhotos { get; set; }
        }
    }
}