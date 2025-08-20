using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using product_catalog_ecommerce.Models;

namespace product_catalog_ecommerce.Data.Repositories
{
    public class CategoryRepository
    {
        public IEnumerable<Category> GetAll()
        {
            var dt = DbHelper.ExecuteDataTable("SELECT category_id, name, description FROM Category ORDER BY name");
            var list = new List<Category>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new Category
                {
                    CategoryId = (int)r["category_id"],
                    Name = r["name"].ToString(),
                    Description = r["description"].ToString()
                });
            }
            return list;
        }

        public Category GetById(int id)
        {
            var dt = DbHelper.ExecuteDataTable("SELECT category_id, name, description FROM Category WHERE category_id = @id",
                new SqlParameter("@id", id));
            if (dt.Rows.Count == 0) return null;
            var r = dt.Rows[0];
            return new Category
            {
                CategoryId = (int)r["category_id"],
                Name = r["name"].ToString(),
                Description = r["description"].ToString()
            };
        }

        public int Create(Category cat)
        {
            var sql = "INSERT INTO Category (name, description) VALUES (@name, @desc); SELECT SCOPE_IDENTITY();";
            var obj = DbHelper.ExecuteScalar(sql,
                new SqlParameter("@name", cat.Name),
                new SqlParameter("@desc", (object)cat.Description ?? DBNull.Value));
            return Convert.ToInt32(obj);
        }

        public int Update(Category cat)
        {
            var sql = "UPDATE Category SET name = @name, description = @desc WHERE category_id = @id";
            return DbHelper.ExecuteNonQuery(sql,
                new SqlParameter("@name", cat.Name),
                new SqlParameter("@desc", (object)cat.Description ?? DBNull.Value),
                new SqlParameter("@id", cat.CategoryId));
        }

        public int Delete(int id)
        {
            return DbHelper.ExecuteNonQuery("DELETE FROM Category WHERE category_id = @id", new SqlParameter("@id", id));
        }
    }
}