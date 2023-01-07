using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class BlogDetailModel
    {
        public Blog Blogs { get; set; }
        public List<Category> Categories { get; set; }
    }
}