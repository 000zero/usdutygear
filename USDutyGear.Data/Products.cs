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
                CommandText = $"SELECT * FROM products WHERE is_active = 1 AND {field} = @value;"
            };
            cmd.Parameters.AddWithValue("@value", value);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            if (dt.Rows.Count < 1)
                return null;

            var row = dt.AsEnumerable().First();
            return ConvertRowToProduct(row);
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
                DependentModelsRegexStr = Convert.ToString(row["dependent_models"]),
                Type = Convert.ToString(row["type"]),
                Name = Convert.ToString(row["name"]),
                PriceAdjustment = Convert.ToDecimal(row["price_adjustment"]),
                Model = Convert.ToString(row["model"]),
                Priority = Convert.ToInt32(row["priority"]),
                Display = Convert.ToString(row["display"])
            }).ToList();
        }

        public static List<string> GetProductDetailsByName(string model)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = "SELECT detail FROM product_details WHERE model = @model;"
            };
            cmd.Parameters.AddWithValue("@model", model);
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
            const string query = @"
                SELECT 
	                models.model AS singleModel,  
                    IF(packages.model IS NULL, NULL, CONCAT(models.model, '-', packages.model)) AS packageModel
                FROM (
	                SELECT 
		                CONCAT(
			                p.model,
			                IF(finishes.model IS NOT NULL, CONCAT('-', finishes.model), ''),
			                IF(buckles.model IS NOT NULL, CONCAT('-', buckles.model), ''),
			                IF(snaps.model IS NOT NULL, CONCAT('-', snaps.model), ''),
                            IF(innerLiners.model IS NOT NULL, CONCAT('-', innerLiners.model), ''),
			                IF(sizes.model IS NOT NULL, CONCAT('-', sizes.model), '')
		                ) AS model
	                FROM products AS p
	                LEFT JOIN product_adjustments AS finishes ON p.model = finishes.product_model AND finishes.type = 'Finish'
	                LEFT JOIN product_adjustments AS buckles ON p.model = buckles.product_model AND buckles.type = 'Buckle'
	                LEFT JOIN product_adjustments AS snaps ON p.model = snaps.product_model AND snaps.type = 'Snap'
	                LEFT JOIN product_adjustments AS sizes ON p.model = sizes.product_model AND sizes.type = 'Size'
	                LEFT JOIN product_adjustments AS innerLiners ON p.model = innerLiners.product_model AND innerLiners.type = 'Inner Liner'
	                WHERE is_active = 1 AND p.model = @model
                ) AS models
                LEFT JOIN product_packages packages ON models.model RLIKE packages.applicable_model
                ORDER BY models.model;";

            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = query
            };
            cmd.Parameters.AddWithValue("@model", model);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            var models = new List<string>();
            foreach (var row in dt.AsEnumerable())
            {
                models.Add(Convert.ToString(row["singleModel"]));

                var packageModel = Convert.ToString(row["packageModel"]);
                if (!string.IsNullOrWhiteSpace(packageModel))
                    models.Add(packageModel);
            }

            return models.Distinct().OrderBy(x => x).ToList();
        } 

        public static ProductPackage GetProductPackage(string model)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"
                    SELECT * FROM product_packages
                    WHERE @model RLIKE applicable_model;"
            };
            cmd.Parameters.AddWithValue("@model", model);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            if (dt.Rows.Count < 1)
                return null;

            return new ProductPackage
            {
                Name = Convert.ToString(dt.Rows[0]["name"]),
                ProductModel = Convert.ToString(dt.Rows[0]["product_model"]),
                Model = Convert.ToString(dt.Rows[0]["model"]),
                Price = Convert.ToDecimal(dt.Rows[0]["price"]),
                ApplicableModelRegexStr = Convert.ToString(dt.Rows[0]["applicable_model"]),
            };
        }

        public static List<ProductPackage> GetProductPackages(string model)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"
                    SELECT * FROM product_packages
                    WHERE product_model = @model;"
            };
            cmd.Parameters.AddWithValue("@model", model);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            if (dt.Rows.Count < 1)
                return null;

            return dt.AsEnumerable().Select(row => new ProductPackage
            {
                Name = Convert.ToString(row["name"]),
                ProductModel = Convert.ToString(row["product_model"]),
                Model = Convert.ToString(row["model"]),
                Price = Convert.ToDecimal(row["price"]),
                ApplicableModelRegexStr = Convert.ToString(row["applicable_model"]),
            }).ToList();
        }

        public static List<ProductCategory> GetProductCategories()
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"
                    SELECT DISTINCT category, name, model
                    FROM products
                    WHERE is_active = 1
                    ORDER BY category, name;"
            };
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            var categories =
                from row in dt.AsEnumerable()
                group row by Convert.ToString(row["category"])
                into grp
                select new ProductCategory
                {
                    Category = grp.Key,
                    Products = grp.Select(x => new KeyValuePair<string, string>(
                        Convert.ToString(x["name"]), Convert.ToString(x["model"]))).ToList()
                };

            return categories.ToList();
        }

        public static List<Product> GetProducts(string category = null)
        {
            switch (category?.ToLower())
            {
                case "belts":
                    category = Categories.Belts;
                    break;
                case "beltkeepers":
                    category = Categories.BeltKeepers;
                    break;
                case "pouches":
                    category = Categories.Pouches;
                    break;
            }

            // TODO: logging error handling
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = string.IsNullOrWhiteSpace(category)
                    ? "SELECT * FROM products WHERE is_active = 1;"
                    : "SELECT * FROM products WHERE is_active = 1 AND category = @category;"
            };

            if (!string.IsNullOrWhiteSpace(category))
                cmd.Parameters.AddWithValue("@category", category);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            if (dt.Rows.Count < 1)
                return null;

            return dt.AsEnumerable().Select(ConvertRowToProduct).ToList();
        }

        public static string GetProductTitle(string fullModel)
        {
            var models = fullModel.Split('-');

            if (models.Length < 2)
                return null;

            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = $@"
                    SELECT p.model AS product_model, a.model AS adj_model, A.type, a.name, title 
                    FROM products p
                    JOIN product_adjustments a ON p.model = a.product_model
                    WHERE p.model = @model AND a.model IN ({string.Join(",", models.Skip(1).Select(x => $"'{x}'"))})"
            };
            cmd.Parameters.AddWithValue("@model", models.First());
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            if (dt.Rows.Count < 1)
                return null;

            var title = new StringBuilder();
            foreach (var row in dt.AsEnumerable())
            {
                // init the title
                if (title.Length == 0)
                    title.Append(row["title"]);
                    

                // do replacement on the adjustment model
                title.Replace($"{{{Convert.ToString(row["type"])}}}", Convert.ToString(row["name"]));
            }

            return title.ToString();
        }

        private static Product ConvertRowToProduct(DataRow row)
        {
            return new Product
            {
                Id = Convert.ToInt32(row["id"]),
                Name = Convert.ToString(row["name"]),
                Category = Convert.ToString(row["category"]),
                Model = Convert.ToString(row["model"]),
                Price = Convert.ToDecimal(row["price"]),
                Title = Convert.ToString(row["title"]),
                ModelTemplate = Convert.ToString(row["model_template"]),
                ModelRegex = new Regex(Convert.ToString(row["model_regex"])),
                DisplayOrder = Convert.ToInt32(row["display_order"]),
                Description = Convert.ToString(row["description"]),
                FeatureImages = Convert.ToString(row["feature_images"]).Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList()
            };
        }
    }
}
