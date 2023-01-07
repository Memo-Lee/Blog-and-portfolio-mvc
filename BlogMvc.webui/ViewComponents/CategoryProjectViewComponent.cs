using BlogMvc.data.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace BlogMvc.webui.ViewComponents
{
    public class CategoryProjectViewComponent:ViewComponent
    {
        private ICategoryProjectRepository _categoryprojectRepository;
        public CategoryProjectViewComponent(ICategoryProjectRepository categoryprojectRepository)
        {
            this._categoryprojectRepository=categoryprojectRepository;
        }
        public IViewComponentResult Invoke()
        {
            //null değilse category getir
            if(RouteData.Values["categorypj"]!=null)
            ViewBag.SelectedProjectCategories=RouteData?.Values["categorypj"];
            return View(_categoryprojectRepository.GetAll());
        }
    }
}