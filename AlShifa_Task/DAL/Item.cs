using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlShifa_Task.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }

}