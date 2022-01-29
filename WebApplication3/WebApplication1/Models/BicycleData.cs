using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public static class BicycleData
    {
        public static void Init(BicycleContext context) 
        {
            if(!context.Bicycles.Any())
            {
                context.Bicycles.AddRange(
                    new Bicycle
                    {
                        Model= "Cross Focus 26",
                        Country="UA",
                        Color="Gray-Blue",
                        Weight = 14,
                        ImgUrl= "https://content1.rozetka.com.ua/goods/images/big_tile/174538047.jpg",
                        Price=6100
                    },
                    new Bicycle
                    {
                        Model = "CrossBike Leader 11",
                        Country = "UA",
                        Color = "Orange",
                        Weight = 15,
                        ImgUrl = "https://content2.rozetka.com.ua/goods/images/big_tile/172266530.jpg",
                        Price = 5500
                    },
                    new Bicycle
                    {
                        Model = "Comanche Niagara Comp 26",
                        Country = "UA",
                        Color = "Blue",
                        Weight = 12,
                        ImgUrl = "https://content.rozetka.com.ua/goods/images/base_action/60659113.jpg",
                        Price = 11000
                    },
                    new Bicycle
                    {
                        Model = "BIANCHI Duel 29",
                        Country = "IT",
                        Color = "Yellow/Black",
                        Weight = 12,
                        ImgUrl = "https://content1.rozetka.com.ua/goods/images/base_action/193759647.jpg",
                        Price = 11000
                    },
                    new Bicycle
                    {
                        Model = "Cube Race Line LTD AMS 26",
                        Country = "DE",
                        Color = "Black",
                        Weight = 14,
                        ImgUrl = "https://content2.rozetka.com.ua/goods/images/big_tile/207274747.jpg",
                        Price = 22000
                    },
                    new Bicycle
                    {
                        Model = "Orbea Occam H20 - Eagle XL 2021 Metallic Black",
                        Country = "ES",
                        Color = "Black",
                        Weight = 13,
                        ImgUrl = "https://content.rozetka.com.ua/goods/images/big/172266297.jpg",
                        Price = 106000
                    },
                    new Bicycle
                    {
                        Model = "KTM CHICAGO DISC 29",
                        Country = "AT",
                        Color = "Orange/Black",
                        Weight = 14,
                        ImgUrl = "https://content1.rozetka.com.ua/goods/images/base_action/6327410.jpg",
                        Price = 24000
                    },
                    new Bicycle
                    {
                        Model = "SCOTT Scale 970",
                        Country = "CH",
                        Color = "Red",
                        Weight = 13,
                        ImgUrl = "https://content.rozetka.com.ua/goods/images/base_action/193760020.jpg",
                        Price = 39900
                    },
                    new Bicycle
                    {
                        Model = "KINK BMX Kicker 18",
                        Country = "US",
                        Color = "Gray",
                        Weight = 11,
                        ImgUrl = "https://content2.rozetka.com.ua/goods/images/base_action/108846805.jpg",
                        Price = 12000
                    },
                    new Bicycle
                    {
                        Model = "Giant Liv Enchant 26",
                        Country = "CN",
                        Color = "Black",
                        Weight = 15,
                        ImgUrl = "https://content.rozetka.com.ua/goods/images/base_action/218442207.jpg",
                        Price = 8000
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
