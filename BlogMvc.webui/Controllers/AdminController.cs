using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data;
using BlogMvc.data.Abstract;
using BlogMvc.entity;
using BlogMvc.webui.Identity;
using BlogMvc.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static BlogMvc.webui.Models.AboutModel;
using static BlogMvc.webui.Models.BlogViewModel;
using static BlogMvc.webui.Models.ProjectViewModel;

namespace BlogMvc.webui.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private IBlogRepository _blogRepository;
        private IProjectRepository _projectRepository;
        private ICategoryRepository _categoryRepository;
        private ICategoryProjectRepository _categoryprojectRepository;
        private ICategorySkillRepository _categoryskillRepository;
        private IAboutRepository _aboutRepository;
        private ISchoolRepository _schoolRepository;
        private ISkillRepository _skillRepository;
        private IHomeBannerRepository _homebannerRepository;
        private INoiceTextRepository _noicetextRepository;
        private ICareerRepository _careerRepository;
        private ISocialMediaRepository _socialmediaRepository;
        private IProfilePhotoRepository _profilemediaRepository;
        // İnject 
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        public AdminController(
                                IBlogRepository blogRepository,IProjectRepository projectRepository,
                                ICategoryRepository categoryRepository,ICategoryProjectRepository categoryprojectRepository,
                                IAboutRepository aboutRepository,ISchoolRepository schoolRepository,ISkillRepository skillRepository,
                                ICategorySkillRepository categoryskillRepository ,IHomeBannerRepository homebannerRepository,
                                INoiceTextRepository noicetextRepository,ICareerRepository careerRepository,ISocialMediaRepository socialmediaRepository,
                                IProfilePhotoRepository profilephotoRepository,RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _blogRepository=blogRepository;
            _projectRepository=projectRepository;
            _categoryRepository=categoryRepository;
            _categoryprojectRepository=categoryprojectRepository;
            _categoryskillRepository=categoryskillRepository;
            _aboutRepository= aboutRepository;
            _schoolRepository= schoolRepository;
            _skillRepository= skillRepository;
            _homebannerRepository=homebannerRepository;
            _noicetextRepository= noicetextRepository;
            _careerRepository=careerRepository;
            _socialmediaRepository =socialmediaRepository;
            _profilemediaRepository=profilephotoRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }
/* *************************************************Roles************************************** */
        [HttpGet("/home")]
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user!=null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i=>i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel(){
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/user/list");            
        }

        [HttpPost("/home")]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user!=null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // veritabanında kayıtlı olan roles alınıyor.
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles?? new string[]{};
                        // veritabanında olmayan bir rolü seçmişse AddToRole.. db ekleniyor.
                        // Except(hariç) ile veritabanında olmayanları , formdan seçtikleri çıkartılır.
                        await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>()); 
                        // db'de olan kayıtlar içerisinden var olan selectedRoles'ları siliyoruz.
                        await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("/admin/user/list");
                    }
                }
                return Redirect("/admin/user/list");
            }
            return View(model);
        }
        
        public IActionResult UserList()
        {
            
            return View(_userManager.Users);            
        }
        
        // [Authorize(Roles ="Admin")] spesifik işlemlere ekleme yapabiliyoruz.
        [HttpGet("admin/role/{id?}")]
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            // admin olanların listesi
            var members = new List<ApplicationUser>();
            // admin olmayanların listesi
            var nonmembers = new List<ApplicationUser>();

            foreach (var user in _userManager.Users)
            {
                // IsInRoleAsync bool türünde soru soruyoruz
                // true ise members false ise nonmembers atıyoruz.
                var list = await _userManager.IsInRoleAsync(user,role.Name)?members:nonmembers;
                list.Add(user);
            }
            // model oluşturuyoruz ve atama yapıyoruz
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                // eklenecek idleri dolaşalım
                // ?? new string[]{} = eğer null ise elemanı olmayan string bir ifade oluştur
                foreach (var userId in model.IdsToAdd ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user!=null)
                    {
                        // ilgili role name'e role id'yi atıyoruz.
                        var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if (!result.Succeeded)
                        {
                            // hata döndürme
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
                // silinecek idleri dolaşalım
                foreach (var userId in model.IdsToDelete ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user!=null)
                    {
                        // ilgili role name'e role id'yi atıyoruz.
                        var result = await _userManager.RemoveFromRoleAsync(user,model.RoleName);
                        if (!result.Succeeded)
                        {
                            // hata döndürme
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/"+model.RoleId);
        }

        [HttpGet("admin/role/list")]
        public IActionResult RoleList()
        {
            // roles list ulaşmak.
            return View(_roleManager.Roles);
        }
        // [Authorize(Roles ="Admin")] spesifik işlemlere ekleme yapabiliyoruz.
        [HttpGet("admin/role/create")]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost("admin/role/create")]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                // role ismini Identity role olarak bekliyor
                // IdendityRole constructur olarak çağrılır 
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                // result=succeeded true/olumlu dönüş
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }else{
                    // result içinde ki Error'larını foreach ile döndürebiliriz.
                    // diğer işlemlerde tempData ile kullanıcalara hata döndürülebilir.
                    foreach (var error in result.Errors)
                    {
                        // hataların neden olduğunu döndürme
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View();
        }

/* *************************************************Blog************************************** */


/* AdminBlogList Listeleme işlemi */
        [Route("/adminblogs")]
        [Route("/adminblogs/{category}")]
        public IActionResult AdminBlogList(string category,int page=1)
        {
            const int pageSize=7;
            var blogViewModel = new BlogViewModel.BlogListViewModel()
            {
                PageInfo = new BlogViewModel.PageInfo()
                {
                    TotalItems=_blogRepository.GetAll().Count(),
                    CurrentPage=page,
                    ItemsPerPage=pageSize
                },
                Blogs = _blogRepository.GetAdminBlogsByItems(page,pageSize),
            };
            return View(blogViewModel);
        }

/* Blogs Create işlemi */
        [HttpGet("Admin/blogs/create/{id?}")]
        public IActionResult BlogCreate()
        {
            ViewBag.Categories = _categoryRepository.GetAll();
            return View();
        }
        [HttpPost("Admin/blogs/create/")]
        public IActionResult BlogCreate(BlogModel model,int[] categoryIds)
        {
            var entity= new Blog()
            {
                BlogHeader=model.BlogHeader,
                BlogUrl=model.BlogUrl,
                BlogText=model.BlogText,
                BlogDate=model.BlogDate,
                BlogImageUrl= model.BlogImageUrl,
                IsApproved=model.IsApproved,
                IsHome=model.IsHome
            };
            _blogRepository.Create(entity,categoryIds);
            // Redirect --> farklı vir view'e yönlendirme

            // var msg = new AlertMessage()
            // {
            //     Message= $"{entity.BlogHeader} isimli ürün eklendi",
            //     AlertType="success"
            // };
            // TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("AdminBlogList");
        }

/* Blogs Edit işlemi */
        [HttpGet("/Admin/BlogEdit/{id?}")]
        public IActionResult BlogEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var entity= _blogRepository.GetByIdWithCategories((int) id);
            
            if (entity==null)
            {
                return NotFound();
            }
            var model= new BlogModel()
            {
                BlogId=entity.BlogId,
                BlogHeader=entity.BlogHeader,
                BlogUrl=entity.BlogUrl,
                BlogText=entity.BlogText,
                BlogDate=entity.BlogDate,
                BlogImageUrl=entity.BlogImageUrl,
                IsApproved=entity.IsApproved,
                IsHome=entity.IsHome,
                SelectedCategories = entity.BlogCategories.Select(i=>i.Category).ToList()
            };
            ViewBag.Categories= _categoryRepository.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> BlogEdit(BlogModel model,int[] categoryIds,IFormFile file)
        {
            var entity = _blogRepository.GetById(model.BlogId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.BlogHeader=model.BlogHeader;
            entity.BlogUrl=model.BlogUrl;
            entity.BlogText=model.BlogText;
            entity.IsApproved=model.IsApproved;
            entity.IsHome=model.IsHome;
            _blogRepository.Update(entity,categoryIds);
            if (file!=null)
            {
                var extention = Path.GetExtension(file.FileName); // .jpg uzantısı için
                var randomName = string.Format($"{Guid.NewGuid()}{extention}"); //DateTime.Now.Ticks benzersiz isim verir
                entity.BlogImageUrl=randomName; // veritabanına random isim kaydı
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",randomName); // resmin dosyasının fiziksel olarak kayıt edilmesi
                // Path = resim nereye kayıt edilecek uzantı
                // Directory.GetCurrentDirectory() --> webui dizini almak. 1ve2 path girişi
                // elimizdeki dosyayı ve uzantıyı fiziksel olarak klasöre kayıt etme
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    // Asektron keyword---> Task<> parametresi
                }
            }
            ViewBag.Categories = _categoryRepository.GetAll();
            // msg Aletmessage gelcek.
            return RedirectToAction("AdminBlogList");
        }

/* Blogs Delete işlemi */
        public IActionResult BlogDelete(int BlogId)
        {
            var entity= _blogRepository.GetById(BlogId);
            if (entity!=null)
            {
                _blogRepository.Delete(entity);
            }
            // var msg = new AlertMessage()
            // {
            //     Message=$"{entity.BlogHeader} isimli ürün silindi",
            //     AlertType = "danger"
            // };
            // TempData["message"]=JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("AdminBlogList");
        }

/* *************************************************Blog CATEGORY************************************** */

/* Blog Category Listeleme */
        
        [Route("Admin/categories/{id?}")]
        public IActionResult BlogCategoryList()
        {
            return View(new CategoryListViewModel()
            {
                 Categories=_categoryRepository.GetAll()
            });
        }


/* Blog Category Create işlemi */

        [HttpGet("Admin/categories/create/{id?}")]
        // CreateProduct view getirilesi
        public IActionResult BlogCategoryCreate()
        {
            return View();
        }
        [HttpPost("Admin/categories/create/")]
        // CreateProduct view getirilesi
        public IActionResult BlogCategoryCreate(CategoryModel model)
        {
            var entity= new Category()
            {
                Name=model.Name,
                Url=model.Url
            };
            _categoryRepository.Create(entity);
            // Redirect --> farklı bir view'e yönlendirme
            
            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori eklendi.",
            //     AlertType="success"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }

            //ProductList'e döndürüyoruz.
            return RedirectToAction("BlogCategoryList");
        }

/* Blog Category Edit işlemi */
        [HttpGet("/Admin/BlogCategoryEdit/{id?}")]
        // CreateProduct view getirilesi
        public IActionResult BlogCategoryEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            // burdan entity bize gelecek daha sonra sayfaya gönderilecek
            // AdminCategory'de CategoryEdit dediğimizde yanında o kategoriye ait ürünleri getirme işlemi.
            var entity= _categoryRepository.GetByIdWithBlogs((int) id);

            if (entity==null)
            {
                return NotFound();
            }
            // bilgiler modele aktarılacak almış olduğumuz entity içinde product listeside mevcut, listeyi model olarak taşımak için CategoryModel'e Product metodu eklenmeli.
            var model= new CategoryModel()
            {
                CategoryId=entity.CategoryId,
                Name=entity.Name,
                Url=entity.Url,
                //entity üzerinden ProductCategories'e geçiş yapıyoruz ve select ile product seçiyoruz. 2 aşamlı seçme yapıyoruz.
                // Product liste ToList() ile dönderiyoruz.
                Blogs=entity.BlogCategories.Select(p=>p.Blog).ToList()
            }; 

            return View(model);
        }
        
        [HttpPost]
        // CreateProduct view getirilesi
        public IActionResult BlogCategoryEdit(CategoryModel model)
        {
            var entity = _categoryRepository.GetById(model.CategoryId);
            //entity sorgulama
            if (entity==null)
            {
                return NotFound();
            }
            // model'den gelen name entity'e aktarılır.

            entity.CategoryId=model.CategoryId;
            entity.Name=model.Name;
            entity.Url=model.Url;
            
            // güncelleme işlemi uptade
            _categoryRepository.Update(entity);

            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori güncellendi.",
            //     AlertType="success"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }

            return RedirectToAction("BlogCategoryList");
        }
/* Category Delete işlemi */
        public IActionResult DeleteCategory(int CategoryId)
        {
            var entity= _categoryRepository.GetById(CategoryId);
            if (entity!=null)
            {
                _categoryRepository.Delete(entity);
            }

            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori silindi.",
            //     AlertType="danger"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }
            // tekrar listeye döndürme
            return RedirectToAction("BlogCategoryList");
        }


/* *************************************************PROJECTS************************************** */

/* AdminProjectList Listeleme işlemi */
        [Route("/adminprojects")]
        [Route("/adminprojects/{category}")]
         public IActionResult AdminProjectList(string category,int page=1)
        {
            const int pageSize=7;
            var projectViewModel = new ProjectViewModel.ProjectListViewModel()
            {
                PageInfo = new ProjectViewModel.PageInfo()
                {
                    TotalItems=_projectRepository.GetAll().Count(),
                    CurrentPage=page,
                    ItemsPerPage=pageSize
                },
                Projects = _projectRepository.GetAdminProjectsByItems(page,pageSize),
            };
            return View(projectViewModel);
        }

/* Projects Create işlemi */
        [HttpGet("Admin/projects/create/{id?}")]
        public IActionResult ProjectCreate()
        {
            ViewBag.CategoryProjects = _categoryprojectRepository.GetAll();
            return View();
        }
        [HttpPost("Admin/projects/create/")]
        public IActionResult ProjectCreate(ProjectModel model,int[] categoryIds)
        {
            var entity = new Project()
            {
                ProjectHeader=model.ProjectHeader,
                ProjectUrl=model.ProjectUrl,
                ProjectText=model.ProjectText,
                ProjectImageUrl=model.ProjectImageUrl,
                IsApproved = model.IsApproved,
                IsHome = model.IsHome
            };
                _projectRepository.Create(entity,categoryIds);

            // if (file!=null)
            // {
            //     var extention = Path.GetExtension(file.FileName); // .jpg uzantısı için
            //     var randomname = string.Format($"{Guid.NewGuid()}{extention}"); //DateTime.Now.Ticks benzersiz isim verir
            //     model.ProjectImageUrl=randomname; // veritabanına random isim kaydı
            //     var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",file.FileName); // resmin dosyasının fiziksel olarak kayıt edilmesi
            //     // Path = resim nereye kayıt edilecek uzantı
            //     // Directory.GetCurrentDirectory() --> webui dizini almak. 1ve2 path girişi
            //     // elimizdeki dosyayı ve uzantıyı fiziksel olarak klasöre kayıt etme
            //     using (var stream = new FileStream(path,FileMode.Create))
            //     {
            //         await file.CopyToAsync(stream);
            //         // Asektron keyword---> Task<> parametresi
            //     }
            // }
            
            // Redirect --> farklı vir view'e yönlendirme

            // var msg = new AlertMessage()
            // {
            //     Message= $"{entity.BlogHeader} isimli ürün eklendi",
            //     AlertType="success"
            // };
            // TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("AdminProjectList");
        }

/* Projects Edit işlemi */
        [HttpGet("/Admin/ProjectEdit/{id?}")]
        public IActionResult ProjectEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var entity= _projectRepository.GetByIdWithProjectCategories((int) id);
            
            if (entity==null)
            {
                return NotFound();
            }
            var model= new ProjectModel()
            {
                ProjectId=entity.ProjectId,
                ProjectHeader=entity.ProjectHeader,
                ProjectUrl=entity.ProjectUrl,
                ProjectText=entity.ProjectText,
                ProjectImageUrl=entity.ProjectImageUrl,
                IsApproved=entity.IsApproved,
                IsHome=entity.IsHome,
                SelectedProjectCategories = entity.ProjectCategories.Select(i=>i.CategoryPj).ToList()
            };
            ViewBag.CategoryProjects = _categoryprojectRepository.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProjectEdit(ProjectModel model,int[] categoryIds,IFormFile file)
        {
            var entity = _projectRepository.GetById(model.ProjectId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.ProjectHeader=model.ProjectHeader;
            entity.ProjectUrl=model.ProjectUrl;
            entity.ProjectText=model.ProjectText;
            entity.IsApproved=model.IsApproved;
            entity.IsHome=model.IsHome;
            _projectRepository.Update(entity,categoryIds);

            if (file!=null)
            {
                var extention = Path.GetExtension(file.FileName); // .jpg uzantısı için
                var randomname = string.Format($"{Guid.NewGuid()}{extention}"); //DateTime.Now.Ticks benzersiz isim verir
                entity.ProjectImageUrl=randomname; // veritabanına random isim kaydı
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",file.FileName); // resmin dosyasının fiziksel olarak kayıt edilmesi
                // Path = resim nereye kayıt edilecek uzantı
                // Directory.GetCurrentDirectory() --> webui dizini almak. 1ve2 path girişi
                // elimizdeki dosyayı ve uzantıyı fiziksel olarak klasöre kayıt etme
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    // Asektron keyword---> Task<> parametresi
                }
            }
            // msg Aletmessage gelcek.
            return RedirectToAction("AdminProjectList");
        }
/* Projects Delete işlemi */
        public IActionResult ProjectDelete(int ProjectId)
        {
            var entity= _projectRepository.GetById(ProjectId);
            if (entity!=null)
            {
                _projectRepository.Delete(entity);
            }
            // var msg = new AlertMessage()
            // {
            //     Message=$"{entity.BlogHeader} isimli ürün silindi",
            //     AlertType = "danger"
            // };
            // TempData["message"]=JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("AdminProjectList");
        }

/* *************************************************PROJECT CATEGORY************************************** */

/* PROJECT Category Listeleme */
        
        [Route("Admin/categoryprojects/{id?}")]
        public IActionResult ProjectCategoryList()
        {
            return View(new CategoryProjectListViewModel()
            {
                 CategoryProjects=_categoryprojectRepository.GetAll()
            });
        }


/* PROJECT Category Create işlemi */

        [HttpGet("Admin/projectcategory/create/{id?}")]
        // CreateProduct view getirilesi
        public IActionResult ProjectCategoryCreate()
        {
            return View();
        }

        [HttpPost("Admin/projectcategory/create/")]
        // CreateProduct view getirilesi
        public IActionResult ProjectCategoryCreate(CategoryProjectModel model)
        {
            var entity= new CategoryPj()
            {
                Name=model.Name,
                Url=model.Url
            };
            _categoryprojectRepository.Create(entity);
            // Redirect --> farklı bir view'e yönlendirme
            
            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori eklendi.",
            //     AlertType="success"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }

            //ProductList'e döndürüyoruz.
            return RedirectToAction("ProjectCategoryList");
        }

/* PROJECT Category Edit işlemi */
        [HttpGet("/Admin/ProjectCategoryEdit/{id?}")]
        // CreateProduct view getirilesi
        public IActionResult ProjectCategoryEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            // burdan entity bize gelecek daha sonra sayfaya gönderilecek
            // AdminCategory'de CategoryEdit dediğimizde yanında o kategoriye ait ürünleri getirme işlemi.
            var entity = _categoryprojectRepository.GetByIdWithProjects((int) id);

            if (entity==null)
            {
                return NotFound();
            }
            // bilgiler modele aktarılacak almış olduğumuz entity içinde product listeside mevcut, listeyi model olarak taşımak için CategoryModel'e Product metodu eklenmeli.
            var model= new CategoryProjectModel()
            {
                CategoryProjectId=entity.CategoryProjectId,
                Name=entity.Name,
                Url=entity.Url,
                //entity üzerinden ProductCategories'e geçiş yapıyoruz ve select ile product seçiyoruz. 2 aşamlı seçme yapıyoruz.
                // Product liste ToList() ile dönderiyoruz.
                Projects=entity.ProjectCategories.Select(p=>p.Project).ToList()
            }; 

            return View(model);
        }
        
        [HttpPost]
        // CreateProduct view getirilesi
        public IActionResult ProjectCategoryEdit(CategoryProjectModel model)
        {
            var entity = _categoryprojectRepository.GetById(model.CategoryProjectId);
            //entity sorgulama
            if (entity==null)
            {
                return NotFound();
            }
            // model'den gelen name entity'e aktarılır.

            entity.CategoryProjectId=model.CategoryProjectId;
            entity.Name=model.Name;
            entity.Url=model.Url;
            
            // güncelleme işlemi uptade
            _categoryprojectRepository.Update(entity);

            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori güncellendi.",
            //     AlertType="success"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }

            return RedirectToAction("ProjectCategoryList");
        }
/* PROJECT Category Delete işlemi */
        public IActionResult CategoryProjectDelete(int CategoryProjectId)
        {
            var entity= _categoryprojectRepository.GetById(CategoryProjectId);
            if (entity!=null)
            {
                _categoryprojectRepository.Delete(entity);
            }

            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori silindi.",
            //     AlertType="danger"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }
            // tekrar listeye döndürme
            return RedirectToAction("ProjectCategoryList");
        }
/* *************************************************About************************************** */
/* About Menu Listeleme işlemi */

[Route("about/aboutmenu/{id?}")]
        public IActionResult AboutMenu()
        {
            return View();
        }

/* About Listeleme işlemi */
        [Route("Admin/abouts/{id?}")]
        public IActionResult AboutList()
        {
            return View(new AboutListViewModel(){

                Abouts=_aboutRepository.GetAll()
            });
        }
/* About Create işlemi */
        [HttpGet("Admin/abouts/create/{id?}")]
        public IActionResult AboutCreate()
        {
            return View();
        }
        [HttpPost("Admin/abouts/create/")]
        public IActionResult AboutCreate(AboutModel model)
        {
            var entity= new About()
            {
                AboutBaslik=model.AboutBaslik,
                AboutText=model.AboutText,
                IsApproved=model.IsApproved
            };
            _aboutRepository.Create(entity);
            // Redirect --> farklı vir view'e yönlendirme

            // var msg = new AlertMessage()
            // {
            //     Message= $"{entity.BlogHeader} isimli ürün eklendi",
            //     AlertType="success"
            // };
            // TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("Aboutlist");
        }

/* About Edit işlemi */
         [HttpGet("/Admin/AboutEdit/{id?}")]
        public IActionResult AboutEdit(int? id)
        { 
            if (id==null)
            {
                return NotFound();
            }

            var entity = _aboutRepository.GetById((int) id);

            if (entity==null)
            {
                return NotFound();
            }

            var model= new AboutModel()
            {
                AboutBaslikId=entity.AboutBaslikId,
                AboutBaslik=entity.AboutBaslik,
                AboutText=entity.AboutText,
                IsApproved=entity.IsApproved,
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AboutEdit(AboutModel model)
        {
            var entity = _aboutRepository.GetById(model.AboutBaslikId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.AboutBaslik=model.AboutBaslik;
            entity.AboutText=model.AboutText;
            entity.IsApproved=model.IsApproved;
            _aboutRepository.Update(entity);
            // msg Aletmessage gelcek.
            return RedirectToAction("AboutList");
        }

/* About Delete işlemi */
        public IActionResult AboutDelete (int AboutBaslikId)
        {
            var entity= _aboutRepository.GetById(AboutBaslikId);
            if (entity!=null)
            {
                _aboutRepository.Delete(entity);
            }
            // var msg = new AlertMessage()
            // {
            //     Message=$"{entity.BlogHeader} isimli ürün silindi",
            //     AlertType = "danger"
            // };
            // TempData["message"]=JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("AboutList");
        }

/* *************************************************School************************************** */

/* School Listeleme işlemi */
        [Route("Admin/schools/{id?}")]
        public IActionResult SchoolList()
        {
            return View(new AboutListViewModel(){

                Schools=_schoolRepository.GetAll()
            });
        }
/* School Create işlemi */
        [HttpGet("Admin/schools/create/{id?}")]
        public IActionResult SchoolCreate()
        {
            return View();
        }
        [HttpPost("Admin/schools/create/{id?}")]
        public IActionResult SchoolCreate(SchoolModel model)
        {
            var entity= new School()
            {
                SchoolId=model.SchoolId,
                SchoolUrl=model.SchoolUrl,
                SchoolName=model.SchoolName,
                SchoolEpisode=model.SchoolEpisode,
                SchoolLisans=model.SchoolLisans,
                SchoolYear=model.SchoolYear,
            };
            _schoolRepository.Create(entity);
            // Redirect --> farklı vir view'e yönlendirme

            // var msg = new AlertMessage()
            // {
            //     Message= $"{entity.BlogHeader} isimli ürün eklendi",
            //     AlertType="success"
            // };
            // TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("SchoolList");
        }

/* School Edit işlemi */
        [HttpGet("/Admin/SchoolEdit/{id?}")]
        public IActionResult SchoolEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var entity = _schoolRepository.GetById((int) id);
            
            if (entity==null)
            {
                return NotFound();
            }
            var model= new SchoolModel()
            {
                SchoolId=entity.SchoolId,
                SchoolUrl=entity.SchoolUrl,
                SchoolName=entity.SchoolName,
                SchoolEpisode=entity.SchoolEpisode,
                SchoolLisans=entity.SchoolLisans,
                SchoolYear=entity.SchoolYear,
                IsApproved=entity.IsApproved
            };
            ViewBag.Categories= _categoryRepository.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult SchoolEdit(SchoolModel model)
        {
            var entity = _schoolRepository.GetById(model.SchoolId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.SchoolUrl=model.SchoolUrl;
            entity.SchoolName=model.SchoolName;
            entity.SchoolEpisode=model.SchoolEpisode;
            entity.SchoolLisans=model.SchoolLisans;
            entity.SchoolYear=model.SchoolYear;
            entity.IsApproved=model.IsApproved;
            _schoolRepository.Update(entity);

            // msg Aletmessage gelcek.
            return RedirectToAction("SchoolList");
        }

/* School Delete işlemi */
        public IActionResult SchoolDelete(int SchoolId)
        {
            var entity= _schoolRepository.GetById(SchoolId);
            if (entity!=null)
            {
                _schoolRepository.Delete(entity);
            }
            // var msg = new AlertMessage()
            // {
            //     Message=$"{entity.BlogHeader} isimli ürün silindi",
            //     AlertType = "danger"
            // };
            // TempData["message"]=JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("SchoolList");
        }
/* ************************************************* SKİLLS ************************************** */

        [Route("/adminskills")]
        [Route("/adminskills/{category}")]
        public IActionResult AdminSkillList(string category,int page=1)
        {
            const int pageSize=7;
            var skillViewModel = new SkillVİewModel.SkillListViewModel()
            {
                PageInfo = new SkillVİewModel.PageInfo()
                {
                    TotalItems=_skillRepository.GetAll().Count(),
                    CurrentPage=page,
                    ItemsPerPage=pageSize
                },
                Skills = _skillRepository.GetAdminSkillsByItems(page,pageSize),
            };
            return View(skillViewModel);
        }

/* SKİLLS Create işlemi */
        [HttpGet("Admin/skills/create/{id?}")]
        public IActionResult SkillCreate()
        {
            return View();
        }
        [HttpPost("Admin/skills/create/")]
        public IActionResult SkillCreate(SkillModel model)
        {
            var entity= new Skill()
            {
                SkillText=model.SkillText,
                SkillPoint=model.SkillPoint,
                Url=model.Url,
                IsApproved=model.IsApproved
            };
            _skillRepository.Create(entity);
            // Redirect --> farklı vir view'e yönlendirme

            // var msg = new AlertMessage()
            // {
            //     Message= $"{entity.BlogHeader} isimli ürün eklendi",
            //     AlertType="success"
            // };
            // TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("AdminSkillList");
        }

/* SKİLLS Edit işlemi */
         [HttpGet("/Admin/SkillEdit/{id?}")]
        public IActionResult SkillEdit(int? id)
        { 
            if (id==null)
            {
                return NotFound();
            }

            var entity = _skillRepository.GetByIdWithSkillCategories((int) id);

            if (entity==null)
            {
                return NotFound();
            }

            var model= new SkillModel()
            {
                SkillId=entity.SkillId,
                SkillText=entity.SkillText,
                SkillPoint=entity.SkillPoint,
                Url=entity.Url,
                IsApproved=entity.IsApproved,
                SelectedSkillCategories = entity.SkillCategories.Select(i=>i.CategorySkill).ToList()
            };
            ViewBag.categorySkills = _categoryskillRepository.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult SkillEdit(SkillModel model,int[] categoryIds)
        {
            var entity = _skillRepository.GetById(model.SkillId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.SkillText=model.SkillText;
            entity.SkillPoint=model.SkillPoint;
            entity.Url=model.Url;
            entity.IsApproved=model.IsApproved;
            _skillRepository.Update(entity,categoryIds);
            // msg Aletmessage gelcek.
            return RedirectToAction("AdminSkillList");
        }

/* SKİLLS Delete işlemi */
        public IActionResult SkillDelete(int SkillId)
        {
            var entity= _skillRepository.GetById(SkillId);
            if (entity!=null)
            {
                _skillRepository.Delete(entity);
            }
            // var msg = new AlertMessage()
            // {
            //     Message=$"{entity.BlogHeader} isimli ürün silindi",
            //     AlertType = "danger"
            // };
            // TempData["message"]=JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("AdminSkillList");
        }

/* *************************************************Skill CATEGORY************************************** */

/* Skill Category Listeleme */
        
        [Route("Admin/categoryskills/{id?}")]
        public IActionResult SkillCategoryList()
        {
            return View(new CategoryListViewSkillModel()
            {
                 CategorySkills=_categoryskillRepository.GetAll()
            });
        }


/* Skill Category Create işlemi */

        [HttpGet("Admin/skillcategory/create/{id?}")]
        // CreateProduct view getirilesi
        public IActionResult SkillCategoryCreate()
        {
            return View();
        }
        [HttpPost("Admin/skillcategory/create/")]
        // CreateProduct view getirilesi
        public IActionResult SkillCategoryCreate(CategorySkillModel model)
        {
            var entity= new CategorySkill()
            {
                SkillArea=model.SkillArea,
                Url=model.Url,
                IsApproved=model.IsApproved
            };
            _categoryskillRepository.Create(entity);
            // Redirect --> farklı bir view'e yönlendirme
            
            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori eklendi.",
            //     AlertType="success"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }

            //ProductList'e döndürüyoruz.
            return RedirectToAction("SkillCategoryList");
        }

/* Skill Category Edit işlemi */
        [HttpGet("/Admin/SkillCategoryEdit/{id?}")]
        // CreateProduct view getirilesi
        public IActionResult SkillCategoryEdit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            // burdan entity bize gelecek daha sonra sayfaya gönderilecek
            // AdminCategory'de CategoryEdit dediğimizde yanında o kategoriye ait ürünleri getirme işlemi.
            var entity= _categoryskillRepository.GetByIdWithSkills((int) id);

            if (entity==null)
            {
                return NotFound();
            }
            // bilgiler modele aktarılacak almış olduğumuz entity içinde product listeside mevcut, listeyi model olarak taşımak için CategoryModel'e Product metodu eklenmeli.
            var model= new CategorySkillModel()
            {
                CategorySkillId=entity.CategorySkillId,
                SkillArea=entity.SkillArea,
                Url=entity.Url,
                IsApproved=entity.IsApproved,
                //entity üzerinden ProductCategories'e geçiş yapıyoruz ve select ile product seçiyoruz. 2 aşamlı seçme yapıyoruz.
                // Product liste ToList() ile dönderiyoruz.
                Skills=entity.SkillCategories.Select(p=>p.Skill).ToList()
            }; 

            return View(model);
        }
        
        [HttpPost]
        // CreateProduct view getirilesi
        public IActionResult SkillCategoryEdit(CategorySkillModel model)
        {
            var entity = _categoryskillRepository.GetById(model.CategorySkillId);
            //entity sorgulama
            if (entity==null)
            {
                return NotFound();
            }
            // model'den gelen name entity'e aktarılır.

            entity.CategorySkillId=model.CategorySkillId;
            entity.SkillArea=model.SkillArea;
            entity.Url=model.Url;
            entity.IsApproved=model.IsApproved;
            // güncelleme işlemi uptade
            _categoryskillRepository.Update(entity);

            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori güncellendi.",
            //     AlertType="success"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }

            return RedirectToAction("SkillCategoryList");
        }
/* Skill Category Delete işlemi */
        public IActionResult CategorySkillDelete(int CategorySkillId)
        {
            var entity= _categoryskillRepository.GetById(CategorySkillId);
            if (entity!=null)
            {
                _categoryskillRepository.Delete(entity);
            }

            // var msg = new AlertMessage()
            // {
            //     Message = $"{entity.Name} isimli kategori silindi.",
            //     AlertType="danger"
            // };
            // TempData["message"] =  JsonConvert.SerializeObject(msg);
            // SERİLİZE İŞLEMİ {"Message" :  ".... ürün eklenidi" , "AlertType" : "danger" }
            // tekrar listeye döndürme
            return RedirectToAction("SkillCategoryList");
        }


/* ************************************************* HOMEBANNER ************************************** */
/* HOMEMENU Listeleme işlemi */
        [Route("home/homemenu/{id?}")]
        public IActionResult HomeMenu()
        {
            return View();
        }

/* HOMEBANNER Listeleme işlemi */
        [Route("Admin/homebanners/{id?}")]
        public IActionResult HomeBannerList()
        {
            return View(new HomeBannerListViewModel(){

                HomeBanners=_homebannerRepository.GetAll()
            });
        }
/* HOMEBANNER Create işlemi */
        [HttpGet("Admin/homebanners/create/{id?}")]
        public IActionResult HomeBannerCreate()
        {
            return View();
        }
        [HttpPost("Admin/homebanners/create/")]
        public async Task<IActionResult> HomeBannerCreate(HomeBannerModel model,IFormFile file)
        {
            var entity= new HomeBanner()
            {
                BannerHeader=model.BannerHeader,
                BannerText=model.BannerText,
                Bannerİmage=model.Bannerİmage,
                IsHome=model.IsHome,

            };
            if (file!=null)
            {
                model.Bannerİmage=file.FileName; // BlogImageUrl'e BlogEditten gelen filename veritabanına aktarma
                var extention = Path.GetExtension(file.FileName); // .jpg uzantısı için
                var randomname = string.Format($"{Guid.NewGuid()}{extention}"); //DateTime.Now.Ticks benzersiz isim verir
                model.Bannerİmage=randomname; // veritabanına random isim kaydı
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",file.FileName); // resmin dosyasının fiziksel olarak kayıt edilmesi
                // Path = resim nereye kayıt edilecek uzantı
                // Directory.GetCurrentDirectory() --> webui dizini almak. 1ve2 path girişi

                // elimizdeki dosyayı ve uzantıyı fiziksel olarak klasöre kayıt etme
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    // Asektron keyword---> Task<> parametresi
                }
            }
            _homebannerRepository.Create(entity);

            // Redirect --> farklı vir view'e yönlendirme

            // var msg = new AlertMessage()
            // {
            //     Message= $"{entity.BlogHeader} isimli ürün eklendi",
            //     AlertType="success"
            // };
            // TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("HomeBannerList");
        }

/* HOMEBANNER Edit işlemi */
         [HttpGet("/Admin/HomeBannerEdit/{id?}")]
        public IActionResult HomeBannerEdit(int? id)
        { 
            if (id==null)
            {
                return NotFound();
            }
            var entity = _homebannerRepository.GetById((int) id);

            if (entity==null)
            {
                return NotFound();
            }

            var model= new HomeBannerModel()
            {
                BannerId=entity.BannerId,
                BannerHeader=entity.BannerHeader,
                BannerText=entity.BannerText,
                Bannerİmage=entity.Bannerİmage,
                IsHome=entity.IsHome,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> HomeBannerEdit(HomeBannerModel model,IFormFile file)
        {
            var entity = _homebannerRepository.GetById(model.BannerId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.BannerHeader=model.BannerHeader;
            entity.BannerText=model.BannerText;
            entity.IsHome=model.IsHome;
            _homebannerRepository.Update(entity);
            // msg Aletmessage gelcek.
            if (file!=null)
            {
                entity.Bannerİmage=file.FileName; // BlogImageUrl'e BlogEditten gelen filename veritabanına aktarma
                var extention = Path.GetExtension(file.FileName); // .jpg uzantısı için
                var randomname = string.Format($"{Guid.NewGuid()}{extention}"); //DateTime.Now.Ticks benzersiz isim verir
                entity.Bannerİmage=randomname; // veritabanına random isim kaydı
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",file.FileName); // resmin dosyasının fiziksel olarak kayıt edilmesi
                // Path = resim nereye kayıt edilecek uzantı
                // Directory.GetCurrentDirectory() --> webui dizini almak. 1ve2 path girişi

                // elimizdeki dosyayı ve uzantıyı fiziksel olarak klasöre kayıt etme
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    // Asektron keyword---> Task<> parametresi
                }

            }
            return RedirectToAction("HomeBannerList");
        }

/* HOMEBANNER Delete işlemi */
        public IActionResult HomeBannerDelete(int BannerId)
        {
            var entity= _homebannerRepository.GetById(BannerId);
            if (entity!=null)
            {
                _homebannerRepository.Delete(entity);
            }
            // var msg = new AlertMessage()
            // {
            //     Message=$"{entity.BlogHeader} isimli ürün silindi",
            //     AlertType = "danger"
            // };
            // TempData["message"]=JsonConvert.SerializeObject(msg);
            
            return RedirectToAction("HomeBannerList");
        }


/* *************************************************NoiceText Header************************************** */
/* NoiceText Edit işlemi */
         [HttpGet("/Admin/NoiceTextEdit/{id?}")]
        public IActionResult NoiceTextEdit(int? id)
        { 
            if (id==null)
            {
                return NotFound();
            }
            var entity = _noicetextRepository.GetById((int) id);

            if (entity==null)
            {
                return NotFound();
            }

            var model= new NoiceTextModel()
            {
                NoiceId=entity.NoiceId,
                NoiceContent=entity.NoiceContent,
                IsHome=entity.IsHome
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult NoiceTextEdit(NoiceTextModel model)
        {
            var entity = _noicetextRepository.GetById(model.NoiceId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.NoiceContent=model.NoiceContent;
            entity.IsHome=model.IsHome;
            _noicetextRepository.Update(entity);

            // msg Aletmessage gelcek.
            return RedirectToAction("HomeMenu");
        }


/* *************************************************Career************************************** */

/* Career Listeleme işlemi */
        
        [Route("Admin/careers/{id?}")]
        public IActionResult CareerList()
        {
            return View(new AboutListViewModel(){
                
                Careers =_careerRepository.GetAll()
            });
        }

/* Career Create işlemi */       
        public IActionResult CareerCreate()

        {
            return View();
        }

        [HttpPost("Admin/careers/create/")]
        public IActionResult CareerCreate(CareerModel model)
        {
            var entity= new Career()
            {
                BusinessCompany=model.BusinessCompany,
                BusinessName=model.BusinessName,
                BusinessTime=model.BusinessTime,
                IsApproved=model.IsApproved,

            };
            _careerRepository.Create(entity);

            return RedirectToAction("CareerList");
        }

/* Career Edit işlemi */
         [HttpGet("/Admin/CareerEdit/{id?}")]
        public IActionResult CareerEdit(int? id)
        { 
            if (id==null)
            {
                return NotFound();
            }
            var entity = _careerRepository.GetById((int) id);

            if (entity==null)
            {
                return NotFound();
            }

            var model= new CareerModel()
            {
                BusinessId=entity.BusinessId,
                BusinessCompany=entity.BusinessCompany,
                BusinessName=entity.BusinessName,
                BusinessTime=entity.BusinessTime,
                IsApproved=entity.IsApproved,
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult CareerEdit(CareerModel model)
        {
            var entity = _careerRepository.GetById(model.BusinessId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.BusinessCompany=model.BusinessCompany;
            entity.BusinessName=model.BusinessName;
            entity.BusinessTime=model.BusinessTime;
            entity.IsApproved=model.IsApproved;
            _careerRepository.Update(entity);

            return RedirectToAction("CareerList");
        }


/* Career Delete işlemi */
        public IActionResult CareerDelete(int BusinessId)
        {
            var entity= _careerRepository.GetById(BusinessId);
            
            if (entity!=null)
            {
                _careerRepository.Delete(entity);
            }
            
            return RedirectToAction("CareerList");
        }




/* *************************************************SocialMedia Links************************************** */

/* SocialMedia Listeleme işlemi */
        
        [Route("Admin/socialmedias/{id?}")]
        public IActionResult SocialMediaList()
        {
            return View(new HomeBannerListViewModel(){
                
                SocialMedias =_socialmediaRepository.GetAll()
            });
        }

/* SocialMedia Create işlemi */       
        public IActionResult SocialMediaCreate()
        {
            return View();
        }

        [HttpPost("Admin/socialmedias/create/")]
        public IActionResult SocialMediaCreate(SocialMediaModel model)
        {
            var entity= new SocialMedia()
            {
                SocialMediaUrl=model.SocialMediaUrl,
                SocialMediaIcon=model.SocialMediaIcon,
                IsApproved=model.IsApproved,

            };

            _socialmediaRepository.Create(entity);

            return RedirectToAction("SocialMediaList");
        }

/* SocialMedia Edit işlemi */
         [HttpGet("/Admin/SocialMediaEdit/{id?}")]
        public IActionResult SocialMediaEdit(int? id)
        { 
            if (id==null)
            {
                return NotFound();
            }
            var entity = _socialmediaRepository.GetById((int) id);

            if (entity==null)
            {
                return NotFound();
            }

            var model= new SocialMediaModel()
            {
                SocialMediaId=entity.SocialMediaId,
                SocialMediaUrl=entity.SocialMediaUrl,
                SocialMediaIcon=entity.SocialMediaIcon,
                IsApproved=entity.IsApproved,
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult SocialMediaEdit(SocialMediaModel model)
        {
            var entity = _socialmediaRepository.GetById(model.SocialMediaId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.SocialMediaUrl=model.SocialMediaUrl;
            entity.SocialMediaIcon=model.SocialMediaIcon;
            entity.IsApproved=model.IsApproved;
            _socialmediaRepository.Update(entity);

            return RedirectToAction("SocialMediaList");
        }


/* SocialMedia Delete işlemi */
        public IActionResult SocialMediaDelete(int SocialMediaId)
        {
            var entity= _socialmediaRepository.GetById(SocialMediaId);
            
            if (entity!=null)
            {
                _socialmediaRepository.Delete(entity);
            }
            
            return RedirectToAction("SocialMediaList");
        }


/* *************************************************ProfilePhoto************************************** */

/* ProfilePhoto Edit işlemi */
         [HttpGet("/Admin/ProfilPhotoEdit/{id?}")]
        public IActionResult ProfilPhotoEdit(int? id)
        { 
            if (id==null)
            {
                return NotFound();
            }
            var entity = _profilemediaRepository.GetById((int) id);

            if (entity==null)
            {
                return NotFound();
            }
            var model= new ProfilePhotoModel()
            {
                ProfilePhotoId=entity.ProfilePhotoId,
                ProfilePhotoUrl=entity.ProfilePhotoUrl,
                IsApproved=entity.IsApproved,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProfilePhotoEdit(ProfilePhotoModel model,IFormFile file)
        {
            var entity = _profilemediaRepository.GetById(model.ProfilePhotoId);
            
            if (entity==null)
            {
                return NotFound();
            }
            entity.IsApproved=model.IsApproved;
            _profilemediaRepository.Update(entity);
            // msg Aletmessage gelcek.
            if (file!=null)
            {
                entity.ProfilePhotoUrl=file.FileName; // BlogImageUrl'e BlogEditten gelen filename veritabanına aktarma
                var extention = Path.GetExtension(file.FileName); // .jpg uzantısı için
                var randomname = string.Format($"{Guid.NewGuid()}{extention}"); //DateTime.Now.Ticks benzersiz isim verir
                entity.ProfilePhotoUrl=randomname; // veritabanına random isim kaydı
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",file.FileName); // resmin dosyasının fiziksel olarak kayıt edilmesi
                // Path = resim nereye kayıt edilecek uzantı
                // Directory.GetCurrentDirectory() --> webui dizini almak. 1ve2 path girişi
                // elimizdeki dosyayı ve uzantıyı fiziksel olarak klasöre kayıt etme
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    // Asektron keyword---> Task<> parametresi
                }
            }
            return RedirectToAction("HomeBannerList");
        }






















































    }
}