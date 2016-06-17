using System;
using System.Collections.Generic;
using System.Linq;
using USDutyGear.Core.Models;

namespace USDutyGear.Models
{
    public class ProductPageViewModel
    {
        private Product _product;

        private ProductPageViewModel(Product product)
        {
            _product = product;
        }

        public static ProductPageViewModel Decorate(Product product)
        {
            return new ProductPageViewModel(product);
        }

        public int Id { get { return _product.Id; } }
        public string Name { get { return _product.Name; } }
        public string Category { get { return _product.Category; } }
        public string Model { get { return _product.Model; } }
        public decimal Price { get { return _product.Price; } }
        public string Sku { get { return _product.Sku; } }
        public List<string> Details { get { return _product.Details; } }
        public List<string> Finishes { get { return _product.Finishes; } }
    }
}