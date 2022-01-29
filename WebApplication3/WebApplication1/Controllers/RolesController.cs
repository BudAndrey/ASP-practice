﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.ViewModles;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication_Identity_2.Controllers
{
    [Authorize(Roles ="admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<User> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Roles");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description); 
                    }
                } 
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete (string id)
        {

                IdentityRole role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    await roleManager.DeleteAsync(role);
                    
                }
            return RedirectToAction("Index");
        }
        public IActionResult UserList()
        {
            return View(userManager.Users.ToList());
        }
        public async Task<IActionResult> Edit(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var allRoles = roleManager.Roles.ToList();
                var userRoles = await userManager.GetRolesAsync(user);
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId=user.Id,
                    UserEmail = user.Email,
                    UserRoles=userRoles,
                    AllRoles=allRoles
                };
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit (string userId, List<string> roles)
        {
            User user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                await userManager.AddToRolesAsync(user, addedRoles);
                await userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("UserList");            
                
            }
            return NotFound();
        }
        public async Task<IActionResult> DeleteUser(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return RedirectToAction("UserList");
            }
            return NotFound();
        }
    }
}
