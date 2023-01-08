using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreSchoolRepository : EfCoreGenericRepository<School>, ISchoolRepository
    {
        public EfCoreSchoolRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public List<School> GetPageSchool()
        {
                return BlogContext.Schools
                                .Where(i=>i.IsApproved).ToList();
            
        }
    }
}