using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCategoryProjectRepository : EfCoreGenericRepository<CategoryPj, BlogContext>, ICategoryProjectRepository
    {
        public CategoryPj GetByIdWithProjects(int CategoryId)
        {
            using (var context = new BlogContext())
            {
                return context.CategoryProjects
                                    .Where(i=>i.CategoryProjectId==CategoryId)
                                    .Include(i=>i.ProjectCategories)
                                    .ThenInclude(i=>i.Project)
                                    .FirstOrDefault();
            }
        }

        public List<CategoryPj> GetPopularCategories()
        {
            throw new System.NotImplementedException();
        }
    }
}