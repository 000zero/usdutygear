using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using USDutyGear.Core.Models;
using USDutyGear.Core.Common;

namespace USDutyGear.Data
{
    public class Products
    {
        private const string ConnectionString =
            "Server=MYSQL5011.Smarterasp.net;Database=db_9f5a66_usdgts;Uid=9f5a66_usdgts;Pwd=flores2016;";

        public static Product GetProductByName(string name)
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

            if (dt.Rows.Count < 1)
                return null;

            var row = dt.AsEnumerable().First();
            return new Product
            {
                Id = Convert.ToInt32(row["id"]),
                Name = Convert.ToString(row["name"]),
                Category = Convert.ToString(row["category"]),
                Model = Convert.ToString(row["model"]),
                Price = Convert.ToDecimal(row["price"]),
                Title = Convert.ToString(row["title"]),
                ModelTemplate = new Regex(Convert.ToString(row["model_template"]))
            };
        }

        public static List<ProductAdjustment> GetProductAdjustmentsByModel(string model)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = "SELECT * FROM product_adjustments WHERE product_model = @model;"
            };
            cmd.Parameters.AddWithValue("@model", model);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable().Select(row => new ProductAdjustment
            {
                Id = Convert.ToString(row["id"]),
                ProductName = Convert.ToString(row["product_name"]),
                ProductModel = Convert.ToString(row["product_model"]),
                Type = Convert.ToString(row["type"]),
                Name = Convert.ToString(row["name"]),
                PriceAdjustment = Convert.ToDecimal(row["price_adjustment"]),
                Model = Convert.ToString(row["model"]),
                Priority = Convert.ToInt32(row["priority"]),
                Display = Convert.ToString(row["display"])
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
                CommandText = "SELECT detail FROM product_details WHERE name = @name;"
            };
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable().Select(row => Convert.ToString(row["detail"])).ToList();
        }

        public static Dictionary<string, string[]> GetProductImagesByModel(string model)
        {
            var dt = new DataTable();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                var cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = @"
                        SELECT model, GROUP_CONCAT(DISTINCT path ORDER BY priority) AS paths FROM (
                            SELECT CONCAT(model, '-', adjustment_model) AS model, path, priority FROM product_images 
                            WHERE model = '72'
                        ) AS results GROUP BY results.model"
                };

                var adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                conn.Close();
            }

            return dt
                .AsEnumerable()
                .ToDictionary(
                    row => Convert.ToString(row["model"]), 
                    row => Convert.ToString(row["paths"])?.Split(','));
        }

        public static List<string> GetProductImagesByName(string name)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = "SELECT path FROM product_images WHERE name = @name;"
            };
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable().Select(row => Convert.ToString(row["path"])).ToList();
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
                CommandText = @"SELECT name, title FROM products"
            };
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return dt.AsEnumerable()
                .Select(row => new KeyValuePair<string, List<string>>(
                    Convert.ToString(row["name"]),
                    Convert.ToString(row["title"])
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
