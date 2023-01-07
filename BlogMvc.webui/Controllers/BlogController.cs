using BlogMvc.data.Abstract;
using BlogMvc.entity;
using BlogMvc.webui.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static BlogMvc.webui.Models.BlogViewModel;

namespace BlogMvc.webui.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;
        private ISocialMediaRepository _socialmediaRepository;
        private IProfilePhotoRepository _profilephotoRepository;

        public BlogController(IBlogRepository blogRepository,ISocialMediaRepository socialmediaRepository,IProfilePhotoRepository profilephotoRepository)
        {
            this._blogRepository=blogRepository;
            this._socialmediaRepository = socialmediaRepository;
            this._profilephotoRepository = profilephotoRepository;
        }
    
        [Route("/blogs")]
        [Route("/blogs/{category}")]
        public IActionResult List(string category,int page=1)
        {
            const int pageSize=12;
            var blogViewModel = new BlogViewModel.BlogListViewModel()
            {
                PageInfo=new PageInfo()
                {
                    TotalItems=_blogRepository.GetCountByCategory(category),
                    CurrentPage=page,
                    ItemsPerPage=pageSize,
                    CurrentCategory= category
                },
                Blogs = _blogRepository.GetBlogsByCategory(category,page,pageSize),
                SocialMedias = _socialmediaRepository.GetHomePageSocialMedia(),
                ProfilePhotos = _profilephotoRepository.GetHomePageProfilePhoto()
            };
            
            return View(blogViewModel);
        }
        
        [Route("/blog/details")]
        public IActionResult Details(string url)
        {
            if (url==null)
            {
                return NotFound();
            }
            Blog blog=_blogRepository.GetBlogDetails(url);
            if (blog==null)
            {
                return NotFound();
            }
            return View(new BlogDetailModel{
            Blogs = blog,
            Categories = blog.BlogCategories.Select(i=>i.Category).ToList()
            });
        }
    
    }
}