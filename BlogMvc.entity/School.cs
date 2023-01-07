using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogMvc.entity
{
    public class School
    {
        public int SchoolId { get; set; }
        public string SchoolUrl { get; set; }
        public string SchoolName { get; set; }
        public string SchoolEpisode { get; set; }
        public string SchoolLisans { get; set; }
        public string SchoolYear { get; set; }
        public bool IsApproved { get; set; }

    }
}