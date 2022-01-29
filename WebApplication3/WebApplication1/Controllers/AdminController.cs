using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        BicycleContext context;

        public AdminController(BicycleContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Bicycles.ToList());
        }

        [HttpPost]
        public IActionResult Create(Bicycle bike)
        {
            if(ModelState.IsValid)
            { 
                if (bike.BicycleId==0)
                {
                    context.Bicycles.Add(bike);

                }
                else
                {
                    context.Update(bike);
                }
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bike);
        }
        
        public IActionResult Create(int? id)
        {
            if(id==null)
            {
                return View();
            }
            var bike =context.Bicycles.FirstOrDefault(x => x.BicycleId==id);
            return View(bike);
        }

        [HttpPost]
        public IActionResult Delete (int BicycleId)
        {
            var delItem=context.Bicycles.Find(BicycleId);
            context.Bicycles.Remove(delItem);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
