using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.data.Abstract
{
    public interface ICategoryProjectRepository:IRepository<CategoryPj>
    {
         List<CategoryPj> GetPopularCategories();
         
         CategoryPj GetByIdWithProjects(int CategoryId);
    }
}