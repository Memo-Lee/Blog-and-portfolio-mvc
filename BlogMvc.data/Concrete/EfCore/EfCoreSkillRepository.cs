using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreSkillRepository : EfCoreGenericRepository<Skill, BlogContext>, ISkillRepository
    {
        public Skill GetByIdWithSkillCategories(int id)
        {
            using (var context = new BlogContext())
            {
                return context.Skills
                                    .Where(i=>i.SkillId==id)
                                    .Include(i=>i.SkillCategories)
                                    .ThenInclude(i=>i.CategorySkill)
                                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string Category)
        {
            using (var context = new BlogContext())
            {
                var Skills = context.Skills.Where(i=>i.IsApproved).AsQueryable();
                 // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
                if (!string.IsNullOrEmpty(Category))
                {
                    Skills=Skills
                                        .Include(i=>i.SkillCategories)
                                        .ThenInclude(i=>i.CategorySkill)
                                        .Where(i=>i.SkillCategories.Any(a=>a.CategorySkill.Url==Category));
                // ilk önce join sonra Any metodu ile true,false değeri alıp listeliyoruz.
                }
                return Skills.Count();
            }
        }
        public List<Skill> GetAdminSkillsByItems(int page, int pageSize)
        {
            using(var context = new BlogContext())
            {
                var Skills = context.Skills;
                // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
                return Skills.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }
        public List<Skill> GetPageSkill()
        {
            using (var context = new BlogContext())
            {
                    return context.Skills
                                    .Where(i=>i.IsApproved).ToList();
            }
        }

        

        public void Update(Skill entity, int[] categoryIds)
        {
            using (var context= new BlogContext())
            {
                var skill = context.Skills
                                        .Include(i=>i.SkillCategories)
                                        .FirstOrDefault(i=>i.SkillId==entity.SkillId);

                if (skill!=null)
                {
                    skill.SkillText=entity.SkillText;
                    skill.SkillPoint=entity.SkillPoint;
                    skill.Url=entity.Url;
                    skill.IsApproved=entity.IsApproved;
                    skill.SkillCategories= categoryIds.Select(catid=>new SkillCategory(){
                        SkillId=entity.SkillId,
                        CategorySkillId = catid
                    }).ToList();
                    
                    context.SaveChanges();
                }
            
            }
        }

    }
}




// public List<Skill> GetSkillsByCategory(string name, int page, int pageSize)
// {
//     using(var context = new BlogContext())
//     {
//         var Skills = context
//             .Skills
//             .Where(i=>i.IsApproved)
//             .AsQueryable();
//         // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
//         if (!string.IsNullOrEmpty(name))
//         {
//             Skills=Skills
//                             .Include(i=>i.SkillCategories)
//                             .ThenInclude(i=>i.CategorySkill)
//                             .Where(i=>i.SkillCategories.Any(a=>a.CategorySkill.Url == name));
//         // ilk önce join sonra Any metodu ile true,false değeri alıp listeliyoruz.
//         }
//         return Skills.Skip((page-1)*pageSize).Take(pageSize).ToList();
//     }
// }