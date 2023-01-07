using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreSchoolRepository : EfCoreGenericRepository<School, BlogContext>, ISchoolRepository
    {
        public List<School> GetPageSchool()
        {
            using (var context = new BlogContext())
            {
                    return context.Schools
                                    .Where(i=>i.IsApproved).ToList();
            }
        }
    }
}