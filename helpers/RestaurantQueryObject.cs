using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.helpers
{
    public class RestaurantQueryObject : PaginationQueryObject
    {
        public string? Name { get; set; } = string.Empty;
    }
}