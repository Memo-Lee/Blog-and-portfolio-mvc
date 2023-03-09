using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data.Abstract;
using BlogMvc.data.Concrete.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogMvc.services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBlogRepository,EfCoreBlogRepository>();
            serviceCollection.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();
            serviceCollection.AddScoped<IProjectRepository,EfCoreProjectRepository>();
            serviceCollection.AddScoped<ICategoryProjectRepository,EfCoreCategoryProjectRepository>();
            serviceCollection.AddScoped<IAboutRepository,EfCoreAboutRepository>();
            serviceCollection.AddScoped<ISchoolRepository,EfCoreSchoolRepository>();
            serviceCollection.AddScoped<ISkillRepository,EfCoreSkillRepository>();
            serviceCollection.AddScoped<ICategorySkillRepository,EfCoreCategorySkillRepository>();
            serviceCollection.AddScoped<IHomeBannerRepository,EfCoreHomeBannerRepository>();
            serviceCollection.AddScoped<ICareerRepository,EfCoreCareerRepository>();
            serviceCollection.AddScoped<ISocialMediaRepository,EfCoreSocialMediaRepository>();
            serviceCollection.AddScoped<IProfilePhotoRepository,EfCoreProfilePhotoRepository>();
        }
    }
}