using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class BlogCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}