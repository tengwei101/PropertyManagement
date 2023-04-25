using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManagement
{
    public class PropertyItem
    {
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string Address { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int SquareFeet { get; set; }
        public string Description { get; set; }
        public string PropertyStatus { get; set; }
        public string Owner { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
