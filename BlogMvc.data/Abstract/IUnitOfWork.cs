using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMvc.data.Abstract
{
    public interface IUnitOfWork : IDisposable
    { // Interfaces
        IBlogRepository Products {get;}
        ICategoryRepository Categories {get;}
        IProjectRepository Projects {get;}
        ICategoryProjectRepository CategoryProjects {get;}
        ISkillRepository Skills {get;}
        ICategorySkillRepository CategorySkills {get;}
        IAboutRepository Abouts {get;}
        ICareerRepository Careers {get;}
        ISocialMediaRepository SocialMedias {get;}
        IProfilePhotoRepository ProfilePhoto {get;}
        void Save();
    }
}