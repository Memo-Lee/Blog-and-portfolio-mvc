using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.webui.Identity;
using Microsoft.AspNetCore.Identity;

namespace BlogMvc.webui.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
    }
    public class RoleDetails
    {
        // roller geliyor
        public IdentityRole Role { get; set; }
        // admin role ait olanlar geliyor
        public IEnumerable<ApplicationUser> Members { get; set; }
        // admin role ait olmayanlar geliyor
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
     public class RoleEditModel
    {
        // İlgili role bilgisinin id'si
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        // eklenmek isteyen kullanıcıların id
        public string[] IdsToAdd { get; set; }
        // silinmek isteyen kullanıcıların id
        public string[] IdsToDelete { get; set; }
    }
}