using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.data.Abstract
{
    public interface IProjectRepository:IRepository<Project>
    {
        List<Project> GetHomeProjects();
        List<Project> GetProjectsByCategory(string name, int page, int pageSize);
        List<Project> GetAdminProjectsByItems(int page, int pageSize);
        Project GetProjectDetails(string url);
        int GetCountByCategory(string Category);
        Project GetByIdWithProjectCategories (int id);
        void Update(Project entity,int[] categoryIds);
        void Create(Project entity,int[] categoryIds);
    }
}