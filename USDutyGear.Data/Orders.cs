using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using USDutyGear.Core.Models;
using MySql.Data.MySqlClient;
using USDutyGear.Core.Common;

namespace USDutyGear.Data
{
    public class Orders
    {
        private const string ConnectionString =
            "Server=MYSQL5011.Smarterasp.net;Database=db_9f5a66_usdgts;Uid=9f5a66_usdgts;Pwd=flores2016;";

        public static Order GetOrder(int orderId)
        {
            Order order = null;
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"SELECT * FROM orders WHERE order_id = @order_id"
            };
            cmd.Parameters.AddWithValue("@order_id", orderId);
            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            conn.Close();

            if (dt.Rows.Count > 0)
            {
                conn.Open();
                order = ConvertRowToOrder(dt.Rows[0]);

                // need to get order items now
                var subCmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * FROM order_items WHERE order_id = @order_id"
                };
                subCmd.Parameters.AddWithValue("@order_id", orderId);
                subCmd.ExecuteNonQuery();

                var itemsDt = new DataTable();
                var subAdapter = new MySqlDataAdapter(subCmd);
                subAdapter.Fill(itemsDt);

                if (itemsDt.Rows.Count > 0)
                    order.Items = itemsDt.AsEnumerable().Select(ConvertRowToOrderItem).ToList();

                conn.Close();
            }

            return order;
        }

        public static List<Order> GetOrders(DateTime? start = null, DateTime? end = null, string status = null, int limit = 100)
        {
            var dt = new DataTable();
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = $@"
                    SELECT * FROM orders 
                    WHERE (@start IS NULL OR created > @start) 
                    AND (@end IS NULL OR created < @end) 
                    AND (@status IS NULL OR status = @status) 
                    ORDER BY created
                    LIMIT {limit};"
            };

            // configure search params
            if (start.HasValue)
                cmd.Parameters.AddWithValue("@start", start);
            else
                cmd.Parameters.AddWithValue("@start", DBNull.Value);

            if (end.HasValue)
                cmd.Parameters.AddWithValue("@end", end);
            else
                cmd.Parameters.AddWithValue("@end", DBNull.Value);

            if (!string.IsNullOrWhiteSpace(status))
                cmd.Parameters.AddWithValue("@status", status);
            else
                cmd.Parameters.AddWithValue("@status", DBNull.Value);

            cmd.ExecuteNonQuery();

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            conn.Close();

            var orders = new List<Order>();
            if (dt.Rows.Count > 0)
            {
                foreach (var row in dt.AsEnumerable())
                {
                    conn.Open();
                    var order = ConvertRowToOrder(row);

                    // need to get order items now
                    var subCmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = @"SELECT * FROM order_items WHERE order_id = @order_id"
                    };
                    subCmd.Parameters.AddWithValue("@order_id", order.OrderId);
                    subCmd.ExecuteNonQuery();

                    var itemsDt = new DataTable();
                    var subAdapter = new MySqlDataAdapter(subCmd);
                    subAdapter.Fill(itemsDt);

                    if (itemsDt.Rows.Count > 0)
                        order.Items = itemsDt.AsEnumerable().Select(ConvertRowToOrderItem).ToList();

                    conn.Close();

                    orders.Add(order);
                }
            }

            return orders;
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
                    INSERT INTO order_items(order_id, model, quantity, price, name) 
                    VALUES(@order_id, @model, @quantity, @price, @name);"
                };
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@model", item.Model);
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@price", item.Price);
                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.ExecuteNonQuery();
            }

            return orderId;
        }

        public static void CompleteOrder(int orderId, string upsTracking, string payeezyTransId)
        {
            // set status to complete
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            var cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = @"
                    UPDATE orders 
                    SET status = @status, ups_tracking_id = @ups_tracking_id, payeezy_trans_id = @payeezy_trans_id, completed = UTC_TIMESTAMP()
                    WHERE order_id = @order_id"
            };
            cmd.Parameters.AddWithValue("@status", OrderStatuses.Complete);
            cmd.Parameters.AddWithValue("@ups_tracking_id", upsTracking);
            cmd.Parameters.AddWithValue("@payeezy_trans_id", payeezyTransId);
            cmd.Parameters.AddWithValue("@order_id", orderId);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private static Order ConvertRowToOrder(DataRow row)
        {
            return new Order
            {
                OrderId = Convert.ToInt32(row["order_id"]),
                CartId = Convert.ToString(row["cart_id"]),
                Created = Convert.ToDateTime(row["created"]),
                Completed = string.IsNullOrWhiteSpace(Convert.ToString(row["completed"]))
                    ? null : (DateTime?)Convert.ToDateTime(Convert.ToString(row["completed"])),
                Status = Convert.ToString(row["status"]),
                UpsServiceCode = Convert.ToString(row["ups_service_code"]),
                UpsTrackingId = Convert.ToString(row["ups_tracking_id"]),
                Tax = Convert.ToDecimal(row["tax"]),
                Shipping = Convert.ToDecimal(row["shipping"]),
                ItemTotal = Convert.ToDecimal(row["item_total"]),
                Email = Convert.ToString(row["email"]),
                PayeezyTransId = Convert.ToString(row["payeezy_trans_id"]),
                Name = Convert.ToString(row["name"]),
                Street = Convert.ToString(row["status"]),
                City = Convert.ToString(row["city"]),
                State = Convert.ToString(row["state"]),
                Country = Convert.ToString(row["country"]),
                PostalCode = Convert.ToString(row["postal_code"])
            };
        }

        private static OrderItem ConvertRowToOrderItem(DataRow row)
        {
            return new OrderItem
            {
                OrderItemId = Convert.ToInt32(row["order_item_id"]),
                OrderId = Convert.ToInt32(row["order_id"]),
                Model = Convert.ToString(row["model"]),
                Name = Convert.ToString(row["name"]),
                Quantity = Convert.ToInt32(row["quantity"]),
                Price = row["price"] != DBNull.Value ?  Convert.ToInt32(row["price"]) : 0
            };
        }
    }
}
