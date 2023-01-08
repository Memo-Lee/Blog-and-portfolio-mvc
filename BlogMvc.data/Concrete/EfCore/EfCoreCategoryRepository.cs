using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    { // İşlem daha bitmedi
        public EfCoreCategoryRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public Category GetByIdWithBlogs(int CategoryId)
        {
          
            return BlogContext.Categories
                                .Where(i=>i.CategoryId==CategoryId)
                                .Include(i=>i.BlogCategories)
                                .ThenInclude(i=>i.Blog)
                                .FirstOrDefault();
            
        }

        public List<Category> GetPopularCategories()
        {
            throw new System.NotImplementedException();
        }

        

        

       
    }
}