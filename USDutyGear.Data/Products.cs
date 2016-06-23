using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using USDutyGear.Core.Models;

namespace USDutyGear.Data
{
    public class Products
    {
        private const string ConnectionString =
            "Server=MYSQL5011.Smarterasp.net;Database=db_9f5a66_usdgts;Uid=9f5a66_usdgts;Pwd=flores2016;";

        public static List<Product> GetProductsByName(string name)
        {
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
                Title = Convert.ToString(row["title"]),
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

        protected static Product GetProduct(int id)
        {
            return null;
        }
    }
}
