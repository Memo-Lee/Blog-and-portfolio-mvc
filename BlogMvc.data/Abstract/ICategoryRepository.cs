using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.data.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
         List<Category> GetPopularCategories();
         
         Category GetByIdWithBlogs(int CategoryId);
         
    }
}