using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using USDutyGear.Core.Models;
using USDutyGear.Core.Common;

namespace USDutyGear.Data
{
    public class Products
    {
        private const string ConnectionString =
            "Server=MYSQL5011.Smarterasp.net;Database=db_9f5a66_usdgts;Uid=9f5a66_usdgts;Pwd=flores2016;";

        public static List<Product> GetProductsByName(string name)
        {
            // TODO: logging error handling
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = "SELECT * FROM products WHERE name = @name;"
            };
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable().Select(row => new Product
            {
                Id = Convert.ToInt32(row["id"]),
                Name = Convert.ToString(row["name"]),
                Category = Convert.ToString(row["category"]),
                Model = Convert.ToString(row["model"]),
                Sizes = Convert.ToString(row["sizes"]).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())).ToList(),
                Finish = Convert.ToString(row["finish"]),
                Price = Convert.ToDecimal(row["price"]),
                ShippingCost = Convert.ToDecimal(row["shipping_cost"]),
                Sku = Convert.ToString(row["sku"]),
                Title = Convert.ToString(row["title"])
            }).ToList();
        }

        public static List<string> GetProductDetailsByName(string name)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = "SELECT name FROM product_details WHERE name = @name;"
            };
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable().Select(row => Convert.ToString(row["name"])).ToList();
        }

        public static List<ProductCategory> GetProductCategories()
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = 
                    @"SELECT category, GROUP_CONCAT(name) AS names FROM (
                        SELECT DISTINCT category,name
                        FROM products
                        ORDER BY category,name) as categories
                    GROUP BY category;"
            };
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable().Select(row => new ProductCategory
            {
                Category = Convert.ToString(row["category"]),
                Products = Convert.ToString(row["names"])
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList()
            }).ToList();
        }

        public static List<KeyValuePair<string, List<string>>> GetProductFeatures()
        {
            // TODO: logging error handling
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText =
                    @"SELECT name, GROUP_CONCAT(finish) AS finishes FROM (
	                    SELECT DISTINCT name, finish
                        FROM products) as names
                    GROUP BY name"
            };
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable()
                .Select(row => new KeyValuePair<string, List<string>>(
                    Convert.ToString(row["name"]),
                    Convert.ToString(row["finishes"])
                        .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToList()
                )).ToList();
        }

        protected static Product GetProduct(int id)
        {
            return null;
        }
    }
}
