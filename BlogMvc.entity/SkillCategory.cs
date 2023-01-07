using System.ComponentModel.DataAnnotations;


namespace BlogMvc.entity
{
    public class SkillCategory
    {
        [Key]
        public int CategorySkillId { get; set; }
        public CategorySkill CategorySkill { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}