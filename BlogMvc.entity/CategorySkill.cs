using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMvc.entity
{
    public class CategorySkill
    {
        [Key]
        public int CategorySkillId { get; set; }
        public string SkillArea { get; set; }
        public string Url { get; set; }
        public bool IsApproved { get; set; }
        public List<SkillCategory> SkillCategories { get; set; }
        
    }
}