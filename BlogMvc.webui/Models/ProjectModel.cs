using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class ProjectModel
    {
         public int ProjectId { get; set; }
        [Display(Name ="Header",Prompt ="Blog için başlık giriniz.")]
        public string ProjectUrl { get; set; }
        [Display(Name ="BText",Prompt ="İçeriğinizi burada oluşturunuz.")]
        public string ProjectHeader { get; set; }
        public string ProjectImageUrl { get; set; }
        public string ProjectText { get; set; }
        public DateTime ProjectDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<CategoryPj> SelectedProjectCategories { get; set; }
    }
}