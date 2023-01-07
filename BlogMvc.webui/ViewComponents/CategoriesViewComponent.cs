using BlogMvc.data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogMvc.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesViewComponent(ICategoryRepository categoryRepository)
        {
            this._categoryRepository=categoryRepository;
        }
        public IViewComponentResult Invoke()
        {
            //null değilse category getir
            if(RouteData.Values["category"]!=null)
            ViewBag.SelectedCategories=RouteData?.Values["category"];
            return View(_categoryRepository.GetAll());
        }

    }
}