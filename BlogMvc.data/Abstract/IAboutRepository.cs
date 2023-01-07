using System.Collections.Generic;
using BlogMvc.entity;

namespace BlogMvc.data.Abstract
{
    public interface IAboutRepository: IRepository<About>
    {
        List<About> GetPageAbout();
    }
}