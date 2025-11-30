namespace BombPol.Data.Entities
{
    public class OrderItem : SoftDeleteBusinessModel
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderItem(Guid id, Guid orderId, Guid productId, int quantity, decimal unitPrice, decimal totalPrice)
            : base(id)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
        }
    }
}