using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Extentions;
using WebApplication1.Models;
using WebApplication1.Models.ViewModles;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        BicycleContext context;

        public CartController(BicycleContext context)
        {
            this.context = context;
        }

        public IActionResult Index(string returnUrl)
        {
            var cart = GetCart();
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            } ) ;
        }

        public IActionResult AddToCart (int bikeId,string returnUrl)
        {
            Bicycle bike = context.Bicycles.FirstOrDefault(x => x.BicycleId == bikeId);
            if (bike != null)
            {
                var cart = GetCart();
                cart.AddItem(bike, 1);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index",new {returnUrl});
        }
        public IActionResult RemoveFromCart(int bikeId,string returnUrl)
        {
            Bicycle bike = context.Bicycles.FirstOrDefault(x => x.BicycleId == bikeId);
            if (bike != null)
            {
                var cart = GetCart();
                cart.RemoveLine(bike);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return cart;
        }
    }
}
