using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.entity;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreNoiceTextRepository:EfCoreGenericRepository<NoiceText>, INoiceTextRepository
    {
        public EfCoreNoiceTextRepository(BlogContext context): base(context)
        {
            
        }
        private BlogContext BlogContext{
            get{return context as BlogContext;}
        }
        public List<NoiceText> GetHomePageNoiceText()
        {

            return BlogContext.NoiceTexts
                            .Where(i=>i.IsHome).ToList();
            
        }
        
    }
}