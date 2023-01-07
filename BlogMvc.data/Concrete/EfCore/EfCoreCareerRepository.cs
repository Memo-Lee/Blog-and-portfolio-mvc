using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreCareerRepository:EfCoreGenericRepository<Career,BlogContext>,ICareerRepository
    {
        public List<Career> GetPageCareer()
        {
            using (var context = new BlogContext())
            {
                return context.Careers
                                .Where(i=>i.IsApproved).ToList();
            }
        }
    }
}