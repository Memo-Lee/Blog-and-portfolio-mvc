using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreProjectRepository : EfCoreGenericRepository<Project>, IProjectRepository
    {
        public EfCoreProjectRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
         public Project GetProjectDetails(string url)
        {
            return BlogContext.Projects
                                    .Where(i=>i.ProjectUrl==url)
                                    .Include(i=>i.ProjectCategories)
                                    .ThenInclude(i=>i.CategoryPj)
                                    .FirstOrDefault();
            
        }
        public List<Project> GetProjectsByCategory(string name, int page, int pageSize)
        {
            var Projects = BlogContext
                .Projects
                .Where(i=>i.IsApproved)
                .AsQueryable();
            // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
            if (!string.IsNullOrEmpty(name))
            {
                Projects=Projects
                                .Include(i=>i.ProjectCategories)
                                .ThenInclude(i=>i.CategoryPj)
                                .Where(i=>i.ProjectCategories.Any(a=>a.CategoryPj.Url == name));
            // ilk önce join sonra Any metodu ile true,false değeri alıp listeliyoruz.
            }
            return Projects.Skip((page-1)*pageSize).Take(pageSize).ToList();
            
        }
        public List<Project> GetAdminProjectsByItems(int page, int pageSize)
        {
            var Projects = BlogContext.Projects;
            return Projects.Skip((page-1)*pageSize).Take(pageSize).ToList();
            
        }
        public int GetCountByCategory(string Category)
        {
            var Projects = BlogContext.Projects.Where(i=>i.IsApproved).AsQueryable();
                // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
            if (!string.IsNullOrEmpty(Category))
            {
                Projects= Projects
                                    .Include(i=>i.ProjectCategories)
                                    .ThenInclude(i=>i.CategoryPj)
                                    .Where(i=>i.ProjectCategories.Any(a=>a.CategoryPj.Url==Category));
            // ilk önce join sonra Any metodu ile true,false değeri alıp listeliyoruz.
            }
            return Projects.Count();
            
        }
        
        public List<Project> GetHomeProjects()
        {
            return BlogContext.Projects
                            .Where(i=>i.IsApproved && i.IsHome).ToList();
            
        }

        public Project GetByIdWithProjectCategories(int id)
        {
            return BlogContext.Projects
                                .Where(i=>i.ProjectId==id)
                                .Include(i=>i.ProjectCategories)
                                .ThenInclude(i=>i.CategoryPj)
                                .FirstOrDefault();
            
        }
        public void Update(Project entity, int[] categoryIds)
        {
            var project = BlogContext.Projects
                                    .Include(i=>i.ProjectCategories)
                                    .FirstOrDefault(i=>i.ProjectId==entity.ProjectId);

            if (project!=null)
            {
                project.ProjectHeader=entity.ProjectHeader;
                project.ProjectUrl=entity.ProjectUrl;
                project.ProjectText=entity.ProjectText;
                project.ProjectImageUrl=entity.ProjectImageUrl;
                project.IsApproved=entity.IsApproved;
                project.IsHome=entity.IsHome;
                project.ProjectCategories= categoryIds.Select(catid=>new ProjectCategory(){
                    ProjectId=entity.ProjectId,
                    CategoryProjectId = catid
                }).ToList();
                
                BlogContext.SaveChanges();
            }
            
            
        }

        public void Create(Project entity,int[] categoryIds)
        {

            var project = BlogContext.Projects
                                    .Include(i=>i.ProjectCategories)
                                    .FirstOrDefault(i=>i.ProjectId==entity.ProjectId);
            if (project!=null)
            {
                project.IsHome = entity.IsHome;
                project.IsApproved = entity.IsApproved;
                project.ProjectCategories= categoryIds.Select(catid=>new ProjectCategory(){
                    ProjectId=entity.ProjectId,
                    CategoryProjectId = catid
                }).ToList();
                BlogContext.SaveChanges();
            }
            
            
        }
    }
}