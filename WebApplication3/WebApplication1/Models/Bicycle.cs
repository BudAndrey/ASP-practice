using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Bicycle
    {
        public int? BicycleId { get; set; }
        [Required(ErrorMessage ="Field Model is necessary")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Field Country is necessary")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Field Color is necessary")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Field Weight is necessary")]
        public float Weight { get; set; }
        [Required(ErrorMessage = "Field ImageUrl is necessary")]
        public string ImgUrl { get; set; }
        [Required(ErrorMessage = "Field Price is most important")]
        public int Price { get; set; }
    }
}
