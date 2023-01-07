using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreNoiceTextRepository:EfCoreGenericRepository<NoiceText, BlogContext>, INoiceTextRepository
    {
        public List<NoiceText> GetHomePageNoiceText()
        {
            using (var context = new BlogContext())
            {
                return context.NoiceTexts
                                .Where(i=>i.IsHome).ToList();
            }
        }
        
    }
}