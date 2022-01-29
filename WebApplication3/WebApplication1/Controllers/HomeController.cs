using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.ViewModles;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        BicycleContext context;
        public HomeController(BicycleContext context)
        {
            this.context = context;
        }

        public IActionResult Index( string str="",string country="all", string price = "all", SortType sortType=SortType.None, int? id=0)
        {
            var selectListItems = new List<string> { "all"};
            var selectPriceItems = new List<string> { "all","<10000",">10000" };
            selectListItems.AddRange(context.Bicycles.Select(x => x.Country).Distinct());
            if (!string.IsNullOrEmpty(str))
                ViewBag.Thanks = str;
            if (id < 0)
                id = 0;
            ViewBag.Page = id;
            IEnumerable<Bicycle> result = null;
            switch (sortType)
            {
                case SortType.ModelAsc:
                    result = context.Bicycles.OrderBy(x => x.Model);
                    break;
                case SortType.WeightAsc:
                    result = context.Bicycles.OrderBy(x => x.Weight);
                    break;
                case SortType.PriceAsc:
                    result = context.Bicycles.OrderBy(x => x.Price);
                    break;
                case SortType.CountryAsc:
                    result = context.Bicycles.OrderBy(x => x.Country);
                    break;
                case SortType.ColorAsc:
                    result = context.Bicycles.OrderBy(x => x.Color);
                    break;
                default:
                    result = context.Bicycles;
                    break;
            }
            var list = new List<Bicycle>();
            if (country == "all" && price == "all") list = result.ToList();
            if (country == "all" && price == "<10000") list = result.Where(x => x.Price <= Convert.ToInt32(price.Remove(0, 1))).ToList();
            if (country == "all" && price == ">10000") list = result.Where(x => x.Price > Convert.ToInt32(price.Remove(0, 1))).ToList();
            if (country != "all" && price == "<10000") list = result.Where(x => x.Country.ToLower() == country.ToLower() && x.Price <= Convert.ToInt32(price.Remove(0, 1))).ToList();
            if (country != "all" && price == ">10000") list = result.Where(x => x.Country.ToLower() == country.ToLower() && x.Price > Convert.ToInt32(price.Remove(0, 1))).ToList();
            if (country != "all" && price == "all") list = result.Where(x => x.Country.ToLower() == country.ToLower()).ToList();
            ViewBag.Sort = sortType;
            ViewBag.Filter = country;
            ViewBag.Price = price;
            //var bicycles = context.Bicycles.ToList();
            return View(new BicycleListViewModel { 
                Bicycles= list,
                Countries =new Microsoft.AspNetCore.Mvc.Rendering.SelectList(selectListItems,country),
                Prices = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(selectPriceItems, price)
            });
        }
        public IActionResult Buy(int? id)
        {
            if (id==null ||  id>context.Bicycles.Count())
            {
                return RedirectToAction("Index");
            }
            ViewBag.BicycleId = id;
            var bike= context.Bicycles.FirstOrDefault(x => x.BicycleId == id);
            ViewBag.ImgUrl = bike.ImgUrl;
            return View();
        }


        [HttpPost]
        public IActionResult Buy(Order order)
        {
            if (ModelState.IsValid)
            {
                context.Orders.Add(order);
                context.SaveChanges();
                return RedirectToAction("Index", new { str = "Thank you for your order !!" }); ;
            }
            else
            {
                ViewBag.BicycleId = order.BicycleId;
                ViewBag.ImgUrl = Request.Form["ImgUrl"];
                return View(order);
            }
        }
    }
}
