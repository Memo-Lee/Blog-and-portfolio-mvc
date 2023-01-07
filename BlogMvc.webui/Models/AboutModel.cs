using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class AboutModel
    {
        public int AboutBaslikId { get; set; }
        public string AboutBaslik { get; set; }
        public string AboutText { get; set; }
        public bool IsApproved { get; set; }

    }
    public class SchoolModel
    {
        public int SchoolId { get; set; }
        public string SchoolUrl { get; set; }
        public string SchoolName { get; set; }
        public string SchoolEpisode { get; set; }
        public string SchoolLisans { get; set; }
        public string SchoolYear { get; set; }
        public bool IsApproved { get; set; }
    }
    public class SkillModel
    {
        public int SkillId { get; set; }
        public string SkillText { get; set; }
        public int SkillPoint { get; set; }
        public string Url { get; set; }
        public bool IsApproved { get; set; }
        public List<CategorySkill> SelectedSkillCategories { get; set; }
    }
    public class SkillVİewModel
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
        public class SkillListViewModel
        {
            public PageInfo PageInfo { get; set; }
            public List<Skill> Skills { get; set; }
        }
    }
    public class CareerModel
    {
        public int BusinessId { get; set; }
        public string BusinessCompany { get; set; }
        public string BusinessName { get; set; }
        public string BusinessTime { get; set; }
        public bool IsApproved { get; set; }
    }
    public class AboutListViewModel
    {
        public List<About> Abouts { get; set; } = new();
        public List<School> Schools { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
        public List<Career> Careers { get; set; } = new();
    }

}