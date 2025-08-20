using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace product_catalog_ecommerce.Models
{
    public class CategoryAttribute
    {
        public int CategoryAttributeId { get; set; } // category_attribute_id
        public int CategoryId { get; set; }
        public int AttributeId { get; set; }
        public bool IsRequired { get; set; }
        // helper fields
        public string AttributeName { get; set; }
        public string AttributeDataType { get; set; }
    }
}