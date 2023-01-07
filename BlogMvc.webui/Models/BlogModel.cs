using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogMvc.entity;
using Microsoft.AspNetCore.Http;

namespace BlogMvc.webui.Models
{
    public class BlogModel
    { // BLOG YAZILARINI OLUŞTURMA SİSTEMİ
        public int BlogId { get; set; }
        [Display(Name ="Header",Prompt ="Blog için başlık giriniz.")]
        public string BlogHeader { get; set; }
        public string BlogUrl { get; set; }
        //[Required, Column(TypeName = "Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BlogDate { get; set; }
        [Display(Name ="BText",Prompt ="İçeriğinizi burada oluşturunuz.")]
        public string BlogText { get; set; }
        public string BlogImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}