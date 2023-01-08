using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCategoryProjectRepository : EfCoreGenericRepository<CategoryPj>, ICategoryProjectRepository
    {
        public EfCoreCategoryProjectRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public CategoryPj GetByIdWithProjects(int CategoryId)
        {
            
            return BlogContext.CategoryProjects
                                .Where(i=>i.CategoryProjectId==CategoryId)
                                .Include(i=>i.ProjectCategories)
                                .ThenInclude(i=>i.Project)
                                .FirstOrDefault();
            
        }

        public List<CategoryPj> GetPopularCategories()
        {
            throw new System.NotImplementedException();
        }
    }
}