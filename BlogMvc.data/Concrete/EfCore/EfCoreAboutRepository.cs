using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreAboutRepository : EfCoreGenericRepository<About>, IAboutRepository
    {
        public EfCoreAboutRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public List<About> GetPageAbout()
        {
          
                    return BlogContext.Abouts
                                    .Where(i=>i.IsApproved).ToList();
        }
    }
}