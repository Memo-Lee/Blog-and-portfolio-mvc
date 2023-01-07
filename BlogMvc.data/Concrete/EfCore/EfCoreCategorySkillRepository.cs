using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCategorySkillRepository : EfCoreGenericRepository<CategorySkill, BlogContext>, ICategorySkillRepository
    {
        public CategorySkill GetByIdWithSkills(int CategoryId)
        {
            using (var context = new BlogContext())
            {
                return context.CategorySkills
                                    .Where(i=>i.CategorySkillId==CategoryId)
                                    .Include(i=>i.SkillCategories)
                                    .ThenInclude(i=>i.Skill)
                                    .FirstOrDefault();
            }
        }
    }
}