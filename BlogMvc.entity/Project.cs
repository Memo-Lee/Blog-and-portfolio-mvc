using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class Project
    {   [Key]
        public int ProjectId { get; set; }
        public string ProjectUrl { get; set; }
        public string ProjectHeader { get; set; }
        public string ProjectImageUrl { get; set; }
        public string ProjectText { get; set; }
        public DateTime ProjectDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }
    }
}