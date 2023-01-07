using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, BlogContext>, ICategoryRepository
    { // İşlem daha bitmedi
        public Category GetByIdWithBlogs(int CategoryId)
        {
            using (var context = new BlogContext())
            {
                return context.Categories
                                    .Where(i=>i.CategoryId==CategoryId)
                                    .Include(i=>i.BlogCategories)
                                    .ThenInclude(i=>i.Blog)
                                    .FirstOrDefault();
            }
        }

        public List<Category> GetPopularCategories()
        {
            throw new System.NotImplementedException();
        }

        

        

       
    }
}