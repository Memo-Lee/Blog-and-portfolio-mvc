using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class CategoryPj
    {
        [Key]
        public int CategoryProjectId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }
    }
}