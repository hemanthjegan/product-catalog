using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace product_catalog_ecommerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }     // product_id
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }        // sku
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Attribute values mapped by CategoryAttributeId
        public List<ProductAttributeValue> Attributes { get; set; } = new List<ProductAttributeValue>();
    }
}