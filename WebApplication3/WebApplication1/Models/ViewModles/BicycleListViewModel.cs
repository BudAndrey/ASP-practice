using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Models.ViewModles
{
    public class BicycleListViewModel
    {
        public List<Bicycle> Bicycles { get; set; }
        public SelectList Countries { get; set; }
        public SelectList Prices { get; set; }
    }
}
