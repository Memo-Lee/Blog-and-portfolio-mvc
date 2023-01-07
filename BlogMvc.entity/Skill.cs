using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }
        public string SkillText { get; set; }
        public int SkillPoint { get; set; }
        public string Url { get; set; }
        public bool IsApproved { get; set; }
        public List<SkillCategory> SkillCategories { get; set; }

    }
}