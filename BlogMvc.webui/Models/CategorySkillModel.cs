using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.entity;

namespace BlogMvc.webui.Models
{
    public class CategorySkillModel
    {
        public int CategorySkillId { get; set; }
        public string SkillArea { get; set; }
        public string Url { get; set; }
        public bool IsApproved { get; set; }
        public List<Skill> Skills { get; set; }
    }
    public class CategoryListViewSkillModel
    {
        public List<CategorySkill> CategorySkills { get; set; }

    }

}