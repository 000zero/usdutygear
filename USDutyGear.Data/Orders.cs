using System;
using System.Data;
using USDutyGear.Core.Models;
using MySql.Data.MySqlClient;

namespace USDutyGear.Data
{
    public class Orders
    {
        private const string ConnectionString =
            "Server=MYSQL5011.Smarterasp.net;Database=db_9f5a66_usdgts;Uid=9f5a66_usdgts;Pwd=flores2016;";

        public static Order GetOrder(int orderId)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"
                    SELECT * FROM orders o
                    JOIN order_items i USING (order_id)
                    WHERE order_id = @order_id"
            };
            cmd.Parameters.AddWithValue("@order_id", orderId);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();

            return null;
        }

        public static int SaveOrder(Order order)
        {
            // insert order get ID
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"
                    INSERT INTO orders(cart_id, created, status, ups_service_code, tax, shipping, item_total, email, name, street, city, state, country, postal_code) 
                    VALUES(@cart_id, @created, @status, @ups_service_code, @tax, @shipping, @item_total, @email, @name, @street, @city, @state, @country, @postal_code);
                    SELECT last_insert_id();"
            };
            cmd.Parameters.AddWithValue("@cart_id", order.CartId.ToString());
            cmd.Parameters.AddWithValue("@created", order.Created);
            cmd.Parameters.AddWithValue("@status", order.Status);
            cmd.Parameters.AddWithValue("@ups_service_code", order.UpsServiceCode); 
            cmd.Parameters.AddWithValue("@tax", order.Tax);
            cmd.Parameters.AddWithValue("@shipping", order.Shipping);
            cmd.Parameters.AddWithValue("@item_total", order.ItemTotal);
            cmd.Parameters.AddWithValue("@email", order.Email);
            cmd.Parameters.AddWithValue("@name", order.Name);
            cmd.Parameters.AddWithValue("@street", order.Street);
            cmd.Parameters.AddWithValue("@city", order.City);
            cmd.Parameters.AddWithValue("@state", order.State);
            cmd.Parameters.AddWithValue("@country", order.Country);
            cmd.Parameters.AddWithValue("@postal_code", order.PostalCode);

            var orderId = Convert.ToInt32(cmd.ExecuteScalar());

            foreach (var item in order.Items)
            {
                // insert order items with order ID
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = @"
                    INSERT INTO order_items(order_id, model, quantity) 
                    VALUES(@order_id, @model, @quantity);"
                };
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@model", item.Model);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.ExecuteNonQuery();
            }

            return orderId;
        }

        public static void CompleteOrder(int orderId, string upsTracking)
        {
            // set status to complete

            // update ups tracking id


        }

        //private Order ConvertRowsToProducts(DataRow )
    }
}
