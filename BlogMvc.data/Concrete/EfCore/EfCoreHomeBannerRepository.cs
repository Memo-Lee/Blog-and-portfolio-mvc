using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreHomeBannerRepository : EfCoreGenericRepository<HomeBanner, BlogContext>, IHomeBannerRepository
    {
        public List<HomeBanner> GetHomePageHomeBanner()
        {
            using (var context = new BlogContext())
            {
                return context.HomeBanners
                                .Where(i=>i.IsHome).ToList();
            }
        }
    }
}