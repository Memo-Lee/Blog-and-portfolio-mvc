using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCategorySkillRepository : EfCoreGenericRepository<CategorySkill>, ICategorySkillRepository
    {
        public EfCoreCategorySkillRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public CategorySkill GetByIdWithSkills(int CategoryId)
        {
           
            return BlogContext.CategorySkills
                                .Where(i=>i.CategorySkillId==CategoryId)
                                .Include(i=>i.SkillCategories)
                                .ThenInclude(i=>i.Skill)
                                .FirstOrDefault();
            
        }
    }
}