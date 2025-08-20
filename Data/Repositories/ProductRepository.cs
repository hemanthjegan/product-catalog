using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using product_catalog_ecommerce.Data;
using product_catalog_ecommerce.Models;


namespace YourProject.Data.Repositories
{
    public class ProductRepository
    {
        public IEnumerable<Product> GetAll()
        {
            var sql = "SELECT product_id, category_id, name, sku, price, created_at, updated_at FROM Product ORDER BY name";
            var dt = DbHelper.ExecuteDataTable(sql);
            return MapProducts(dt);
        }

        public Product GetById(int id)
        {
            var dt = DbHelper.ExecuteDataTable("SELECT product_id, category_id, name, sku, price, created_at, updated_at FROM Product WHERE product_id = @id",
                new SqlParameter("@id", id));
            if (dt.Rows.Count == 0) return null;
            var p = MapProducts(dt)[0];

            // load attributes
            var attrDt = DbHelper.ExecuteDataTable(@"
                SELECT pav.product_attribute_value_id, pav.product_id, pav.category_attribute_id, pav.value, a.name as attribute_name
                FROM ProductAttributeValue pav
                JOIN CategoryAttribute ca ON pav.category_attribute_id = ca.category_attribute_id
                JOIN Attribute a ON ca.attribute_id = a.attribute_id
                WHERE pav.product_id = @prodId", new SqlParameter("@prodId", id));
            foreach (DataRow r in attrDt.Rows)
            {
                p.Attributes.Add(new ProductAttributeValue
                {
                    ProductAttributeValueId = (int)r["product_attribute_value_id"],
                    ProductId = (int)r["product_id"],
                    CategoryAttributeId = (int)r["category_attribute_id"],
                    Value = r["value"].ToString(),
                    AttributeName = r["attribute_name"].ToString()
                });
            }
            return p;
        }

        public int Create(Product p)
        {
            var sql = "INSERT INTO Product (category_id, name, sku, price) VALUES (@cat, @name, @sku, @price); SELECT SCOPE_IDENTITY();";
            var obj = DbHelper.ExecuteScalar(sql,
                new SqlParameter("@cat", p.CategoryId),
                new SqlParameter("@name", p.Name),
                new SqlParameter("@sku", p.SKU),
                new SqlParameter("@price", p.Price));
            int newId = Convert.ToInt32(obj);

            // Insert attribute values if any
            foreach (var av in p.Attributes)
            {
                DbHelper.ExecuteNonQuery(
                    "INSERT INTO ProductAttributeValue (product_id, category_attribute_id, value) VALUES (@prod, @catAttr, @val)",
                    new SqlParameter("@prod", newId),
                    new SqlParameter("@catAttr", av.CategoryAttributeId),
                    new SqlParameter("@val", av.Value ?? string.Empty));
            }
            return newId;
        }

        public int Update(Product p)
        {
            var sql = "UPDATE Product SET category_id=@cat, name=@name, sku=@sku, price=@price WHERE product_id=@id";
            var n = DbHelper.ExecuteNonQuery(sql,
                new SqlParameter("@cat", p.CategoryId),
                new SqlParameter("@name", p.Name),
                new SqlParameter("@sku", p.SKU),
                new SqlParameter("@price", p.Price),
                new SqlParameter("@id", p.ProductId));

            // Simplest approach: delete existing values and re-insert
            DbHelper.ExecuteNonQuery("DELETE FROM ProductAttributeValue WHERE product_id = @prod", new SqlParameter("@prod", p.ProductId));
            foreach (var av in p.Attributes)
            {
                DbHelper.ExecuteNonQuery(
                    "INSERT INTO ProductAttributeValue (product_id, category_attribute_id, value) VALUES (@prod, @catAttr, @val)",
                    new SqlParameter("@prod", p.ProductId),
                    new SqlParameter("@catAttr", av.CategoryAttributeId),
                    new SqlParameter("@val", av.Value ?? string.Empty));
            }
            return n;
        }

        private List<Product> MapProducts(DataTable dt)
        {
            var list = new List<Product>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new Product
                {
                    ProductId = (int)r["product_id"],
                    CategoryId = (int)r["category_id"],
                    Name = r["name"].ToString(),
                    SKU = r["sku"].ToString(),
                    Price = Convert.ToDecimal(r["price"]),
                    CreatedAt = Convert.ToDateTime(r["created_at"]),
                    UpdatedAt = Convert.ToDateTime(r["updated_at"])
                });
            }
            return list;
        }
    }
}
