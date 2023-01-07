using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}