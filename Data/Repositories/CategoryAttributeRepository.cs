using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using product_catalog_ecommerce.Data;
using product_catalog_ecommerce.Models;

namespace YourProject.Data.Repositories
{
    public class CategoryAttributeRepository
    {
        public IEnumerable<CategoryAttribute> GetByCategoryId(int categoryId)
        {
            var sql = @"SELECT ca.category_attribute_id, ca.category_id, ca.attribute_id, ca.is_required,
                               a.name as attribute_name, a.data_type
                        FROM CategoryAttribute ca
                        JOIN Attribute a ON ca.attribute_id = a.attribute_id
                        WHERE ca.category_id = @catId
                        ORDER BY a.name";
            var dt = DbHelper.ExecuteDataTable(sql, new SqlParameter("@catId", categoryId));
            var list = new List<CategoryAttribute>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new CategoryAttribute
                {
                    CategoryAttributeId = (int)r["category_attribute_id"],
                    CategoryId = (int)r["category_id"],
                    AttributeId = (int)r["attribute_id"],
                    IsRequired = (bool)r["is_required"],
                    AttributeName = r["attribute_name"].ToString(),
                    AttributeDataType = r["data_type"].ToString()
                });
            }
            return list;
        }

        public int AddMapping(int categoryId, int attributeId, bool isRequired)
        {
            var sql = "INSERT INTO CategoryAttribute (category_id, attribute_id, is_required) VALUES (@cat, @attr, @req); SELECT SCOPE_IDENTITY();";
            var obj = DbHelper.ExecuteScalar(sql,
                new SqlParameter("@cat", categoryId),
                new SqlParameter("@attr", attributeId),
                new SqlParameter("@req", isRequired));
            return Convert.ToInt32(obj);
        }
    }
}
