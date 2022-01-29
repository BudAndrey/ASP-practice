using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Bicycle bike, int qty)
        {
            CartLine line =
                lineCollection.FirstOrDefault(x => x.Bicycle.BicycleId == bike.BicycleId);
            if (line == null)
            {
                lineCollection.Add(new CartLine {Bicycle=bike,Quantity=qty });
            }
            else
            {
                line.Quantity += qty;
            }    
        }
        public void RemoveLine(Bicycle bike)
        {
            lineCollection.RemoveAll(x => x.Bicycle.BicycleId == bike.BicycleId);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public int ComputeTotalValue()
        {
            return lineCollection.Sum(x => x.Quantity * x.Bicycle.Price);
        }
        public IEnumerable<CartLine> Lines
        {
            get => lineCollection;
        }
        public int CountGoods
        {
            get=> lineCollection.Count;
        }
    }
    public class CartLine
    {
        public Bicycle Bicycle { get; set; }
        public int Quantity { get; set; }
    }

}
