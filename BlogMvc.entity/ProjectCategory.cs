using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class ProjectCategory
    {   
        [Key]
        public int CategoryProjectId { get; set; }
        public CategoryPj CategoryPj { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        
    }
}