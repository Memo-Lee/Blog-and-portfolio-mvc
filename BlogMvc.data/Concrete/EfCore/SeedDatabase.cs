
using System;
using System.Linq;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
    
        public static void Seed()
        {
            var context = new BlogContext();
            
                if (context.Database.GetPendingMigrations().Count()==0)
                {
                
                    if (context.Categories.Count() == 0)
                    {
                        context.Categories.AddRange(Categories);
                    }
                    if (context.CategoryProjects.Count() == 0)
                    {
                        context.CategoryProjects.AddRange(CategoryProjects);
                    }
                    if (context.CategorySkills.Count() == 0)
                    {
                        context.CategorySkills.AddRange(CategorySkills);
                    }
                    if (context.Blogs.Count() == 0 )
                    {
                        context.AddRange(Blogs);
                        context.AddRange(BlogCategories);
                    }
                    if (context.Projects.Count()==0)
                    {
                        context.AddRange(Projects);
                        context.AddRange(ProjectCategories);
                    }
                    if (context.Abouts.Count() == 0)
                    {
                        context.AddRange(Abouts);
                    }
                    if (context.Schools.Count() == 0)
                    {
                        context.AddRange(Schools);
                    }
                    if (context.Skills.Count() == 0)
                    {
                        context.AddRange(Skills);
                        context.AddRange(SkillCategories);
                    }
                    if (context.HomeBanners.Count() == 0)
                    {
                        context.AddRange(HomeBanners);
                    }
                    if (context.NoiceTexts.Count() == 0)
                    {
                        context.AddRange(NoiceTexts);
                    }
                    if (context.Careers.Count() == 0)
                    {
                        context.AddRange(Careers);
                    }
                    if (context.SocialMedias.Count() == 0)
                    {
                        context.AddRange(SocialMedias);
                    }
                    if (context.ProfilePhotos.Count() == 0)
                    {
                        context.AddRange(ProfilePhotos);
                    }
                    
                }
                context.SaveChanges();
        }
        private static Category[] Categories = {
            new Category(){Name="Yazılım",Url="yazilim"},
            new Category(){Name="Blokchain",Url="Blokchain"},
            new Category(){Name="İnşaat",Url="insaat"}
        };
        private static CategoryPj[] CategoryProjects = {
            new CategoryPj(){Name="Mobil",Url="mobilcoder"},
            new CategoryPj(){Name="Web",Url="webcoder"},
            new CategoryPj(){Name="Blokchain",Url="blockchain"},
        };
        private static CategorySkill[] CategorySkills = {
            new CategorySkill(){SkillArea="Yazılım",Url="yazilimskill",IsApproved=true},
            new CategorySkill(){SkillArea="İnşaat",Url="insaatskill",IsApproved=true},
            new CategorySkill(){SkillArea="Blokchain",Url="blockchainskill",IsApproved=true},
        };


        private static Blog[] Blogs = {
            new Blog() {BlogHeader="Blokchain nedir?",BlogUrl="blokchain-nedir",BlogText="Çok önemlidir",BlogImageUrl="1.jpg", BlogDate= DateTime.Now, IsApproved=true,IsHome=true},
            new Blog() {BlogHeader="İnşaat nedir?",BlogUrl="insaat-nedir",BlogText="Çok önemlidir",BlogImageUrl="1.jpg",BlogDate=DateTime.Now, IsApproved=true,IsHome=true},
            new Blog() {BlogHeader="Yazılım nedir?",BlogUrl="Yazılım-nedir",BlogText="Çok önemlidir",BlogImageUrl="1.jpg",BlogDate=DateTime.Now, IsApproved=true,IsHome=true},
            new Blog() {BlogHeader="Statik nedir?",BlogUrl="Statik-nedir",BlogText="Çok önemlidir",BlogImageUrl="1.jpg",BlogDate=DateTime.Now, IsApproved=true,IsHome=true},
            
        };
        private static Project[] Projects = {
            new Project() {ProjectHeader="Mobil oyun1",ProjectUrl="mobiloyun",ProjectText="Çok önemlidir",ProjectImageUrl="1.jpg",ProjectDate=DateTime.Now, IsApproved=true,IsHome=true},
            new Project() {ProjectHeader="Mobil oyun2",ProjectUrl="mobiloyun",ProjectText="Çok önemlidir",ProjectImageUrl="1.jpg",ProjectDate=DateTime.Now, IsApproved=true,IsHome=true},
            new Project() {ProjectHeader="Mobil oyun3",ProjectUrl="mobiloyun",ProjectText="Çok önemlidir",ProjectImageUrl="1.jpg",ProjectDate=DateTime.Now, IsApproved=true,IsHome=true},
            new Project() {ProjectHeader="Mobil oyun4",ProjectUrl="mobiloyun",ProjectText="Çok önemlidir",ProjectImageUrl="1.jpg",ProjectDate=DateTime.Now, IsApproved=true,IsHome=true}
        };
        private static About[] Abouts = {
            new About() {AboutBaslik="Benim Hakkımda",AboutText="İstanbulda dogdum",IsApproved=true},
        };
        private static School[] Schools = {
            new School() {SchoolName="Sdü",SchoolEpisode="İnş Müh", SchoolLisans="Lisans",SchoolYear="2018-2022", SchoolUrl="egitimkariyeri",IsApproved=true},
        };
        private static Skill[] Skills = {
            new Skill() {SkillText="C#",SkillPoint=65,Url="yazilimc#",IsApproved=true},
            new Skill() {SkillText="C++",SkillPoint=15,Url="yazilimc++",IsApproved=true},
            new Skill() {SkillText="Java",SkillPoint=45,Url="yazilimjava",IsApproved=true},
            new Skill() {SkillText="React",SkillPoint=35,Url="yazilimreact",IsApproved=true},
        };
        private static HomeBanner[] HomeBanners = {
            new HomeBanner() {BannerHeader="Yazılım",BannerText="C# - C++ - Javascript - Asp.Net", Bannerİmage="1.jpg",IsHome=true}
        };
        private static NoiceText[] NoiceTexts = {
            new NoiceText() {NoiceContent="Yazılımcı arkadaşlar bana ulaşsın",IsHome=true}
        };
        private static Career[] Careers = {
            new Career() {BusinessCompany="Fuzul İnşaat",BusinessName="Civil Engineer",BusinessTime="2018-2019",IsApproved=true}
        };
        private static SocialMedia[] SocialMedias = {
            new SocialMedia() {SocialMediaUrl="www.instagram.com",SocialMediaIcon="",IsApproved=true}
        };
        private static ProfilePhoto[] ProfilePhotos = {
            new ProfilePhoto() {ProfilePhotoUrl="1.jpg",IsApproved=true}
        };

        private static BlogCategory[] BlogCategories={
            new BlogCategory(){Blog=Blogs[0],Category=Categories[0]},
            new BlogCategory(){Blog=Blogs[0],Category=Categories[2]},
            new BlogCategory(){Blog=Blogs[1],Category=Categories[0]},
            new BlogCategory(){Blog=Blogs[1],Category=Categories[2]},
            new BlogCategory(){Blog=Blogs[2],Category=Categories[0]},
            new BlogCategory(){Blog=Blogs[2],Category=Categories[2]},
            new BlogCategory(){Blog=Blogs[3],Category=Categories[0]},
            new BlogCategory(){Blog=Blogs[3],Category=Categories[2]}
        };
        
        private static ProjectCategory[] ProjectCategories={
            new ProjectCategory(){Project=Projects[0],CategoryPj=CategoryProjects[0]},
            new ProjectCategory(){Project=Projects[0],CategoryPj=CategoryProjects[2]},
            new ProjectCategory(){Project=Projects[1],CategoryPj=CategoryProjects[0]},
            new ProjectCategory(){Project=Projects[1],CategoryPj=CategoryProjects[2]},
            new ProjectCategory(){Project=Projects[2],CategoryPj=CategoryProjects[0]},
            new ProjectCategory(){Project=Projects[2],CategoryPj=CategoryProjects[2]},
            new ProjectCategory(){Project=Projects[3],CategoryPj=CategoryProjects[0]},
            new ProjectCategory(){Project=Projects[3],CategoryPj=CategoryProjects[2]}
            
        };
        private static SkillCategory[] SkillCategories={
            new SkillCategory(){Skill=Skills[0],CategorySkill=CategorySkills[0]},
            new SkillCategory(){Skill=Skills[0],CategorySkill=CategorySkills[2]},
            new SkillCategory(){Skill=Skills[1],CategorySkill=CategorySkills[0]},
            new SkillCategory(){Skill=Skills[1],CategorySkill=CategorySkills[2]},
            new SkillCategory(){Skill=Skills[2],CategorySkill=CategorySkills[0]},
            new SkillCategory(){Skill=Skills[2],CategorySkill=CategorySkills[2]},
            new SkillCategory(){Skill=Skills[3],CategorySkill=CategorySkills[0]},
            new SkillCategory(){Skill=Skills[3],CategorySkill=CategorySkills[2]}
        };
    }
    
}

