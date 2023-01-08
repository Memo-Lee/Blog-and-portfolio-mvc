using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreHomeBannerRepository : EfCoreGenericRepository<HomeBanner>, IHomeBannerRepository
    {
        public EfCoreHomeBannerRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public List<HomeBanner> GetHomePageHomeBanner()
        {
            return BlogContext.HomeBanners
                            .Where(i=>i.IsHome).ToList();
        }
    }
}