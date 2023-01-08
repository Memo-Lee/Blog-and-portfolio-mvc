using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreSocialMediaRepository:EfCoreGenericRepository<SocialMedia>,ISocialMediaRepository
    {
        public EfCoreSocialMediaRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public List<SocialMedia> GetHomePageSocialMedia()
        {
           
            return BlogContext.SocialMedias
                            .Where(i=>i.IsApproved).ToList();
            
        }
    }
}