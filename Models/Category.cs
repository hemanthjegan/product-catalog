using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace product_catalog_ecommerce.Models
{
    public class Category
    {
        public int CategoryId { get; set; }     // category_id
        public string Name { get; set; }
        public string Description { get; set; }
    }
}