using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogMvc.data
{
    public class BlogContext : DbContext
    {
        public BlogContext()
        {

        }
        //constructor
        public BlogContext (DbContextOptions options) : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; } // entity ekleme işlemi
        public DbSet<Category> Categories {get; set; } // entity ekleme istelemi
        public DbSet<Project> Projects { get; set; }
        public DbSet<CategoryPj> CategoryProjects {get; set; } // entity ekleme istelemi
        public DbSet<About> Abouts {get; set; }
        public DbSet<School> Schools {get; set; }
        public DbSet<Skill> Skills {get; set; }
        public DbSet<CategorySkill> CategorySkills {get; set; }
        public DbSet<HomeBanner> HomeBanners {get; set; }
        public DbSet<NoiceText> NoiceTexts {get; set; }
        public DbSet<Career> Careers {get; set; }
        public DbSet<SocialMedia> SocialMedias {get; set; }
        public DbSet<ProfilePhoto> ProfilePhotos {get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlite("Data Source=blogDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { // Fluent Api BlogCategory tablosunun birincil öğelerini belirleme işlemi
            modelBuilder.Entity<BlogCategory>()
                            .HasKey(a => new {a.CategoryId , a.BlogId });
            modelBuilder.Entity<ProjectCategory>()
                            .HasKey(a => new {a.CategoryProjectId , a.ProjectId });
            modelBuilder.Entity<SkillCategory>()
                            .HasKey(a => new {a.CategorySkillId , a.SkillId });
        }

        
    }
}