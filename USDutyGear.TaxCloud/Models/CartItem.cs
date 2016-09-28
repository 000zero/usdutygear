namespace USDutyGear.TaxCloud.Models
{
    public class CartItem
    {
        public string ItemID { get; set; }
        public int Index { get; set; }
        public string TIC { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }
}
