using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreProfilePhotoRepository:EfCoreGenericRepository<ProfilePhoto,BlogContext>,IProfilePhotoRepository
    {
        public List<ProfilePhoto> GetHomePageProfilePhoto()
        {
            using (var context = new BlogContext())
            {
                return context.ProfilePhotos
                                .Where(i=>i.IsApproved).ToList();
            }
        }
        
    }
}