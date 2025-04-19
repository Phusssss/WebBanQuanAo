namespace WebBanQuanAo.Models
{
    public class CartItem
    {
        public Product Product { get; set; } = null!; // Sản phẩm
        public int Quantity { get; set; } // Số lượng
    }
}
