using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication44Udemy.Models
{
    public class RestaurantQuery
    {
        public string searchPhrase { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }

        public string SortBy { get; set; }
        public SortingDirection SortingDirection { get; set; }

    }
}
