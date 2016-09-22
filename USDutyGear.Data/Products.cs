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
        private static readonly string[] CommaSep = {","};
        private const string ConnectionString =
            "Server=MYSQL5011.Smarterasp.net;Database=db_9f5a66_usdgts;Uid=9f5a66_usdgts;Pwd=flores2016;";

        public static Product GetProductById(int id)
        {
            return GetProduct("id", id);
        }

        public static Product GetProductByName(string name)
        {
            return GetProduct("name", name);
        }

        public static Product GetProductByModel(string model)
        {
            return GetProduct("model", model);
        }

        private static Product GetProduct<T>(string field, T value)
        {
            // TODO: logging error handling
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = $"SELECT * FROM products WHERE {field} = @value;"
            };
            cmd.Parameters.AddWithValue("@value", value);
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
                ModelTemplate = Convert.ToString(row["model_template"]),
                ModelRegex = new Regex(Convert.ToString(row["model_regex"]))
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
                Display = Convert.ToString(row["display"]),
                DependentModels = Convert.ToString(row["dependent_models"]).Split(CommaSep, StringSplitOptions.RemoveEmptyEntries)
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
                            SELECT IF(adjustment_model IS NULL, model, CONCAT(model, '-', adjustment_model)) AS model, path, priority FROM product_images 
                            WHERE model = @model
                        ) AS results GROUP BY results.model"
                };
                cmd.Parameters.AddWithValue("@model", model);
                cmd.ExecuteNonQuery();

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

        public static List<string> GetPossibleModels(string model)
        {
            var query = "SELECT " +
                "CONCAT(" +
                    "p.model, " +
                    "IF(finishes.model IS NOT NULL, CONCAT('-', finishes.model), ''), " +
                    "IF(buckles.model IS NOT NULL, CONCAT('-', buckles.model), ''), " +
                    "IF(snaps.model IS NOT NULL, CONCAT('-', snaps.model), ''), " +
                    "IF(sizes.model IS NOT NULL, CONCAT('-', sizes.model), ''), " +
                    "IF(packages.model IS NOT NULL AND packages.model != '', CONCAT('-', packages.model), '') " +
                ") AS model " +
FROM products AS p
LEFT JOIN product_adjustments AS finishes ON p.model = finishes.product_model AND finishes.type = 'Finish'
LEFT JOIN product_adjustments AS buckles ON p.model = buckles.product_model AND buckles.type = 'Buckle'
LEFT JOIN product_adjustments AS snaps ON p.model = snaps.product_model AND snaps.type = 'Snap'
LEFT JOIN product_adjustments AS sizes ON p.model = sizes.product_model AND sizes.type = 'Size'
LEFT JOIN product_adjustments AS innerLiners ON p.model = innerLiners.product_model AND innerLiners.type = 'Size'
LEFT JOIN product_adjustments AS packages ON p.model = packages.product_model AND packages.type = 'Package'
WHERE p.model = '72'
ORDER BY model"
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
    }
}
