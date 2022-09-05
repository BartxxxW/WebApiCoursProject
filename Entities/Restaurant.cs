using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication44Udemy.Entities
{
    public class Restaurant
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; }
        public int? CreatedById { get; set; }
        public virtual Users CreatedBy { get; set; }
        public string ContactNumber { get; set; }
        public int AdressId { get; set; }
        public virtual Adress Adress { get; set; }
        public virtual List<Dish> Dishes { get; set; }
    }
}
