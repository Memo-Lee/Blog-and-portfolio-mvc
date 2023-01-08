using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCareerRepository:EfCoreGenericRepository<Career>,ICareerRepository
    {
        public EfCoreCareerRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public List<Career> GetPageCareer()
        {
            
            return BlogContext.Careers
                            .Where(i=>i.IsApproved).ToList();
        
        }
    }
}