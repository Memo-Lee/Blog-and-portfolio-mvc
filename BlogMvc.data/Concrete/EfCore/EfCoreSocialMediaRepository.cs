using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreSocialMediaRepository:EfCoreGenericRepository<SocialMedia,BlogContext>,ISocialMediaRepository
    {
        public List<SocialMedia> GetHomePageSocialMedia()
        {
            using (var context = new BlogContext())
            {
                return context.SocialMedias
                                .Where(i=>i.IsApproved).ToList();
            }
        }
    }
}