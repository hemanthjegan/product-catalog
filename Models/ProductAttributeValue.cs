using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace product_catalog_ecommerce.Models
{
    public class ProductAttributeValue
    {
        public int ProductAttributeValueId { get; set; } // product_attribute_value_id
        public int ProductId { get; set; }
        public int CategoryAttributeId { get; set; }
        public string Value { get; set; }

        // helper fields
        public string AttributeName { get; set; }
    }
}