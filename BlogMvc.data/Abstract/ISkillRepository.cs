using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.entity;

namespace BlogMvc.data.Abstract
{
    public interface ISkillRepository:IRepository<Skill>
    {
        List<Skill> GetPageSkill();
        List<Skill> GetAdminSkillsByItems(int page, int pageSize);
        int GetCountByCategory(string Category);
        Skill GetByIdWithSkillCategories (int id);
        void Update(Skill entity,int[] categoryIds);
         
    }
}