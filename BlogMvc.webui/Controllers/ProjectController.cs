using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using BlogMvc.webui.Models;
using Microsoft.AspNetCore.Mvc;
using static BlogMvc.webui.Models.ProjectViewModel;

namespace BlogMvc.webui.Controllers
{
    public class ProjectController: Controller
    {
        private IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            this._projectRepository=projectRepository;
        }
    
        [Route("/projects")]
        [Route("/projects/{category}")]
        public IActionResult List(string category,int page=1)
        {
            const int pageSize=8;
            var projectViewModel = new ProjectListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems=_projectRepository.GetCountByCategory(category),
                    CurrentPage=page,
                    ItemsPerPage=pageSize,
                    CurrentCategory= category
                },
                Projects= _projectRepository.GetProjectsByCategory(category,page,pageSize),
            };
            return View(projectViewModel);
        }
        [Route("/project/details")]
        public IActionResult Details(string url)
        {
            if (url==null)
            {
                return NotFound();
            }
            Project project =_projectRepository.GetProjectDetails(url);
            if (project==null)
            {
                return NotFound();
            }
            return View(new ProjectDetailModel{
            Project = project,
            CategoryProjects = project.ProjectCategories.Select(i=>i.CategoryPj).ToList()
            });
            
        }
    }
}