using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class ProjectDetailModel
    {
        public Project Project { get; set; }
        public List<CategoryPj> CategoryProjects { get; set; }
    }
}