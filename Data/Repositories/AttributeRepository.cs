using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using product_catalog_ecommerce.Models;

namespace product_catalog_ecommerce.Data.Repositories
{
    public class AttributeRepository
    {
        public IEnumerable<AttributeModel> GetAll()
        {
            var dt = DbHelper.ExecuteDataTable("SELECT attribute_id, name, data_type FROM Attribute ORDER BY name");
            var list = new List<AttributeModel>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new AttributeModel
                {
                    AttributeId = (int)r["attribute_id"],
                    Name = r["name"].ToString(),
                    DataType = r["data_type"].ToString()
                });
            }
            return list;
        }

        public int Create(AttributeModel a)
        {
            var sql = "INSERT INTO Attribute (name, data_type) VALUES (@n, @dt); SELECT SCOPE_IDENTITY();";
            var obj = DbHelper.ExecuteScalar(sql,
                new SqlParameter("@n", a.Name),
                new SqlParameter("@dt", a.DataType));
            return Convert.ToInt32(obj);
        }
    }
}