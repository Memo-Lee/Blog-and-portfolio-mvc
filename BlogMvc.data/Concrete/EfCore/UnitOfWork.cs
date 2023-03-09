using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;

namespace BlogMvc.data.Concrete.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogContext _context;
        public UnitOfWork(BlogContext context)
        {
            _context = context;
        }
        
        public IBlogRepository Products => throw new NotImplementedException();

        public ICategoryRepository Categories => throw new NotImplementedException();

        public IProjectRepository Projects => throw new NotImplementedException();

        public ICategoryProjectRepository CategoryProjects => throw new NotImplementedException();

        public ISkillRepository Skills => throw new NotImplementedException();

        public ICategorySkillRepository CategorySkills => throw new NotImplementedException();

        public IAboutRepository Abouts => throw new NotImplementedException();

        public ICareerRepository Careers => throw new NotImplementedException();

        public ISocialMediaRepository SocialMedias => throw new NotImplementedException();

        public IProfilePhotoRepository ProfilePhoto => throw new NotImplementedException();

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}