using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Your name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Your phone is required")]
        public string Phone { get; set; }
        
        public DateTime Date { get; set; }

        public int? BicycleId { get; set; }
        public Bicycle Bicycle { get; set; }

    }
}
