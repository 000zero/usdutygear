using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Name = Convert.ToString(row["name"])
            }).ToList();
        }

        protected static Product GetProduct(int id)
        {
            return null;
        }
    }
}
