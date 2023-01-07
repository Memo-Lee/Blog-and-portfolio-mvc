using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data;
using BlogMvc.webui.EmailServices;
using BlogMvc.webui.Extensions;
using BlogMvc.webui.Identity;
using BlogMvc.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogMvc.webui.Controllers
{
    // [AutoValidateAntiforgeryToken] // oto get-post controller
    public class AccountController:Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;        
        public AccountController(
                                UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        // [AllowAnonymous] login olmayan kullanıcıların erişebilmesini sağlıyor.
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginModel(){
                // /admin/products gibi url'leri döndürüyoruz.
                ReturnUrl = ReturnUrl
            });
        }       
        [HttpPost]
        // get ile gönderilen post işlemi postlara eklenir.
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // var user = await _userManager.FindByNameAsync(model.UserName);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // user üzerinden arama yapılıyor.
                ModelState.AddModelError("","Bu kullancı adı ile hesap oluşturulmamış");
                return View(model);
            }
            // user mail'i onaylanmamış mı sorgusu
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("","Lütfen email hesabınıza gelen link ile üyeliğinizi onaylayınız.");
                return View(model);
            }

            // cookie işlemleri 3. cookie süresi , 4. hesap kilitleme
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,true,false);
            if (result.Succeeded)
            {
                // iki tane soru işareti '??' null kontrolü yapar
                    // ~/ => home/index'i temsil eder eğer null ise anasayfaya yönlendirir.
                return Redirect(model.ReturnUrl??"~/");
            }
            ModelState.AddModelError("","Girilen kullanıcı adı veya parola yanlış");
            return View();
        } 
        public IActionResult Register()
        {
            return View();
        }       
        
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded)
            {
                // generate token (token oluşturma)
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // url ve token eşleşiyorsa onaylama işlemi
                var url = Url.Action("ConfirmEmail","Account",new{
                    userId = user.Id,
                    token = code
                });
                Console.WriteLine(url);
                // email (email gönderme)
                await _emailSender.SendEmailAsync(model.Email,"Hesabınızı onaylayınız.",$"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:5001{url}'>tıklayınız.</a>");

                return RedirectToAction("Login","Account");
            }
            ModelState.AddModelError("","Bilinmeyen bir hata oldu tekrar deneyiniz...");
            return View(model);
        }       

        public async Task<IActionResult> Logout ()
        {
            // Logout çıkış işlemi
            TempData.Put("message",new AlertMessage(){
                    Title = "Hesabınız kapatıldı.",
                    Message = "Hesabınız güvenli bir şekil de çıkış yaptınız.",
                    AlertType = "warning"
                    });
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail (string userId,string token)
        {
            if (userId==null || token==null) 
            {
                TempData.Put("message",new AlertMessage(){
                    Title = "Geçersiz Token",
                    Message = "Geçersiz Token",
                    AlertType = "danger"
                });

                return View();
            }
            // user control
            var user = await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {   
                // confirmEmail alanını true yapar.
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if (result.Succeeded)
                {
                    TempData.Put("message",new AlertMessage(){
                    Title = "Hesabınız onaylandı.",
                    Message = "Hesabınız onaylandı.",
                    AlertType = "success"
                    });
                    // CreateMessage("Hesabınız onaylandı.","success");
                    return View();
                }
            }
            TempData.Put("message",new AlertMessage(){
                    Title = "Hesabınız onaylanmadı.",
                    Message = "Hesabınız onaylanmadı.",
                    AlertType = "warning"
                    });
            return View();
        }
        
        public IActionResult ForgotPassword ()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword (string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }
            
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // generate token (token oluşturma)
            // url ve token eşleşiyorsa onaylama işlemi
            var url = Url.Action("ResetPassword","Account",new{
                userId = user.Id,
                token = code
            });
            Console.WriteLine(url);
            // email (email gönderme)
            await _emailSender.SendEmailAsync(Email,"Reset Password.",$" Şifrenizi yenilemek için linke <a href='https://localhost:5001{url}'>tıklayınız.</a>");

            return View();
        }
        public IActionResult ResetPassword(string userId,string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Home","Index");
            }
            var model = new ResetPasswordModel {Token = token};

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Home","Index");
            }
            var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login","Account");
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    
    }
}