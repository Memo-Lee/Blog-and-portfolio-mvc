using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.data.Abstract
{
    public interface IBlogRepository : IRepository<Blog> // <T>'ye atama
    {  
        // EfCoreBlogRepository ile entegre çalışır.
       Blog GetBlogDetails(string url);
       Blog GetByIdWithCategories (int id);
       List<Blog> GetBlogsByCategory(string name, int page, int pageSize);
       List<Blog> GetAdminBlogsByItems(int page, int pageSize);
       List<Blog> GetHomePageBlogs();
       int GetCountByCategory(string category);
       void Update(Blog entity,int[] categoryIds);
       void Create(Blog entity,int[] categoryIds);
    }
}