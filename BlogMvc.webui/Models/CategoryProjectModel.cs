using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class CategoryProjectModel
    {
        public int CategoryProjectId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Project> Projects { get; set; }
    }
}