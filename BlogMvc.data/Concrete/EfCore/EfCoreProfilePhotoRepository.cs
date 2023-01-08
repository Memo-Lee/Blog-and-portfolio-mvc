using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreProfilePhotoRepository:EfCoreGenericRepository<ProfilePhoto>,IProfilePhotoRepository
    {
        public EfCoreProfilePhotoRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public List<ProfilePhoto> GetHomePageProfilePhoto()
        {

            return BlogContext.ProfilePhotos
                            .Where(i=>i.IsApproved).ToList();
        }
        
        
    }
}