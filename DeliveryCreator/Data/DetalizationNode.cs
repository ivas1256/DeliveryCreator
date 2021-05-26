using DeliveryCreator.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.Data
{
    class DetalizationNode
    {
        public DateTime Date { get; set; }
        public PriceList PriceList { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }
        public string TNNumber { get; set; }
        public Receiver Receiver { get; set; }

        public override string ToString()
        {
            return $"{Price}; {Weight}кг";
        }
    }
}
