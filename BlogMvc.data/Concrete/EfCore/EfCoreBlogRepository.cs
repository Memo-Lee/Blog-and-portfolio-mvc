using System.Collections.Generic;
using System.Linq;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using Microsoft.EntityFrameworkCore;

namespace BlogMvc.data.Concrete.EfCore
{
    public class EfCoreBlogRepository : EfCoreGenericRepository<Blog, BlogContext>, IBlogRepository
    // TEntity = Blog , DbContext = BlogContext
    // IBlogRepository = Ekstra property (özel) olduğu seçenekler barındırı
    // Diğer propertyler EfCoreGenericRepository içinde bulunur.
    {
        public Blog GetBlogDetails(string url)
        {
            using (var context = new BlogContext())
            {
                return context.Blogs
                                        .Where(i=>i.BlogUrl==url)
                                        .Include(i=>i.BlogCategories)
                                        .ThenInclude(i=>i.Category)
                                        .FirstOrDefault();
            }
        }

        public List<Blog> GetBlogsByCategory(string name, int page, int pageSize)
        {
            using(var context = new BlogContext())
            {
                var Blogs = context
                    .Blogs
                    .Where(i=>i.IsApproved)
                    .AsQueryable();
                // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
                if (!string.IsNullOrEmpty(name))
                {
                    Blogs=Blogs
                                .Include(i=>i.BlogCategories)
                                .ThenInclude(i=>i.Category)
                                .Where(i=>i.BlogCategories.Any(a=>a.Category.Url == name));
                // ilk önce join sonra Any metodu ile true,false değeri alıp listeliyoruz.
                }
                return Blogs.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }
        public List<Blog> GetAdminBlogsByItems(int page, int pageSize)
        {
            using(var context = new BlogContext())
            {
                var Blogs = context.Blogs;
                // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
                return Blogs.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public Blog GetByIdWithCategories(int id)
        {
            using (var context = new BlogContext())
            {
                return context.Blogs
                                    .Where(i=>i.BlogId==id)
                                    .Include(i=>i.BlogCategories)
                                    .ThenInclude(i=>i.Category)
                                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new BlogContext())
            {
                var Blogs = context.Blogs.Where(i=>i.IsApproved).AsQueryable();
                
                 // AsQueryable = name string'i var ise kriter belirleyip sonra ToList ile listeler.
                if (!string.IsNullOrEmpty(category))
                {
                    Blogs=Blogs
                                        .Include(i=>i.BlogCategories)
                                        .ThenInclude(i=>i.Category)
                                        .Where(i=>i.BlogCategories.Any(a=>a.Category.Url==category));
                // ilk önce join sonra Any metodu ile true,false değeri alıp listeliyoruz.
                }
                return Blogs.Count();
            }
        }

        public List<Blog> GetHomePageBlogs()
        {   // Anasayfada gösterilecek ürünlerin IsApproved ve IsHome ları
            using (var context = new BlogContext())
            {
                return context.Blogs
                                .Where(i=>i.IsApproved && i.IsHome).ToList();
            }
        }

        public void Update(Blog entity, int[] categoryIds)
        {
            using (var context= new BlogContext())
            {
                var blog = context.Blogs
                                        .Include(i=>i.BlogCategories)
                                        .FirstOrDefault(i=>i.BlogId==entity.BlogId);

                if (blog!=null)
                {
                    blog.BlogHeader=entity.BlogHeader;
                    blog.BlogUrl=entity.BlogUrl;
                    blog.BlogText=entity.BlogText;
                    blog.BlogDate=entity.BlogDate;
                    blog.BlogImageUrl=entity.BlogImageUrl;
                    blog.IsApproved=entity.IsApproved;
                    blog.IsHome=entity.IsHome;

                    blog.BlogCategories= categoryIds.Select(catid=>new BlogCategory(){
                        BlogId=entity.BlogId,
                        CategoryId = catid
                    }).ToList();
                    
                    context.SaveChanges();
                }
            
            }
        }

         public void Create(Blog entity,int[] categoryIds)
        {
            using (var context= new BlogContext())
            {
                var blog = context.Blogs
                                        .Include(i=>i.BlogCategories)
                                        .FirstOrDefault(i=>i.BlogId==entity.BlogId);
                if (blog!=null)
                {
                    blog.IsHome = entity.IsHome;
                    blog.IsApproved = entity.IsApproved;
                    blog.BlogCategories= categoryIds.Select(catid=>new BlogCategory(){
                        BlogId=entity.BlogId,
                        CategoryId = catid
                    }).ToList();
                    context.SaveChanges();
                }
            
            }
        }
    }
}