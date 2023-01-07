using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.entity;

namespace BlogMvc.data.Abstract
{
    public interface IProfilePhotoRepository:IRepository<ProfilePhoto>
    {
        List<ProfilePhoto> GetHomePageProfilePhoto();
    }
}