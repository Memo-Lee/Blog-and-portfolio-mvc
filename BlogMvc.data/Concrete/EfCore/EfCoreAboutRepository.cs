using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreAboutRepository : EfCoreGenericRepository<About, BlogContext>, IAboutRepository
    {
        public List<About> GetPageAbout()
        {
            using (var context = new BlogContext())
            {
                    return context.Abouts
                                    .Where(i=>i.IsApproved).ToList();
            }
        }
    }
}