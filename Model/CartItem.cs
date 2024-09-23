namespace Cart_Api.Model
{
    public class CartItem
    {
        public int id { get; set; }
        public string product { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
    }
}
