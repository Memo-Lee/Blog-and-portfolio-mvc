using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data;
using BlogMvc.data.Abstract;
using BlogMvc.Models;
using BlogMvc.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static BlogMvc.webui.Models.BlogViewModel;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        // burdaki işlem daha bitmedi
        private readonly IBlogRepository _blogRepository;
        private readonly IHomeBannerRepository _homebannerRepository;
        private readonly INoiceTextRepository _noicetextRepository;
        private readonly ISocialMediaRepository _socialmediaRepository;
        private readonly IProfilePhotoRepository _profilephotoRepository;
        // İnject(enjekte etme) işlemi devamı Startup.cs'de halledildi
        public HomeController(
        IBlogRepository blogRepository,
        IHomeBannerRepository homebannerRepository,
        INoiceTextRepository noicetextRepository,
        ISocialMediaRepository socialmediaRepository,
        IProfilePhotoRepository profilephotoRepository)
        {
            this._blogRepository = blogRepository;
            this._homebannerRepository = homebannerRepository;
            this._noicetextRepository = noicetextRepository;
            this._socialmediaRepository = socialmediaRepository;
            this._profilephotoRepository = profilephotoRepository;
        }
        public IActionResult Index()
        { // ANASAYFAYA GELECEK BLOG YAZILARI İNJECT İŞLEMİ
            var homebannerListViewModel = new HomeBannerListViewModel()
            {
                Blogs = _blogRepository.GetHomePageBlogs(),
                HomeBanners = _homebannerRepository.GetHomePageHomeBanner(),
                NoiceTexts = _noicetextRepository.GetHomePageNoiceText(),
                SocialMedias = _socialmediaRepository.GetHomePageSocialMedia(),
                ProfilePhotos = _profilephotoRepository.GetHomePageProfilePhoto()
            };
            return View(homebannerListViewModel);
        }
        [Route("/home/homemenu")]
        [Authorize]
        public IActionResult HomeMenu()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
