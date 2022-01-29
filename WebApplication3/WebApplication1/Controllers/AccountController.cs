using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.ViewModles;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginViewModel { ReturnUrl=ReturnUrl});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(model.ReturnUrl)&& Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Invalid data entered ");
                }
            }
            return View(model);
        }
       
        public IActionResult Register(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new RegisterViewModel { IsAdmin = false });
            }
            
            else
            {
                return View(new RegisterViewModel { IsAdmin = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user =  new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                    if (!model.IsAdmin)
                    {
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("UserList","Roles");
                    }
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    
                    //return RedirectToAction("Login", "Account");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ChangePasswordAdmin(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAdmin(string id, string password)
        {
            var user = await userManager.FindByIdAsync(id);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded)
            {
                return RedirectToAction("UserList", "Roles");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(user);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ChangePassword(string name)
        {
            var user = await userManager.FindByNameAsync(name);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            var user = await userManager.FindByIdAsync(id);
            
            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                await Logout();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(user);
            }
        }
    }
}
