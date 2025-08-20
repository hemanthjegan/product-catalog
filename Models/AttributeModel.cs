using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace product_catalog_ecommerce.Models
{
    public class AttributeModel
    {
        public int AttributeId { get; set; }    // attribute_id
        public string Name { get; set; }
        public string DataType { get; set; }    // TEXT, NUMBER, BOOLEAN, ENUM
    }
}