using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using BlogMvc.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BlogMvc.webui.Models.AboutModel;

namespace BlogMvc.webui.Controllers
{
    public class AboutController:Controller
    {
        private IAboutRepository _aboutRepository;
        private ISchoolRepository _schoolRepository;
        private ISkillRepository _skillRepository;
        private ICareerRepository _careerRepository;

        public AboutController(IAboutRepository aboutRepository,ISchoolRepository schoolRepository,ISkillRepository skillRepository,ICareerRepository careerRepository)
        {
            this._aboutRepository=aboutRepository;
            this._schoolRepository=schoolRepository;
            this._skillRepository=skillRepository;
            this._careerRepository=careerRepository;
        }
    
        [Route("/abouts")]
        public IActionResult List()
        {   
            var aboutViewModel = new AboutListViewModel()
            {
                Abouts =_aboutRepository.GetPageAbout(),
                Schools = _schoolRepository.GetPageSchool(),
                Skills = _skillRepository.GetPageSkill(),
                Careers = _careerRepository.GetPageCareer()
            };
            
            return View(aboutViewModel);
        }
        [Route("/about/aboutmenu")]
        [Authorize]
        public IActionResult AboutMenu()
        {
            return View();
        }
        public IActionResult Details(string url)
        {
            return View();
            
        }
    }
}