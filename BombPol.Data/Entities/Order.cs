namespace BombPol.Data.Entities
{
    public class Order : SoftDeleteBusinessModel
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order(Guid id, string customerName, string customerEmail, string customerPhone, decimal totalAmount)
            : base(id)
        {
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;
            TotalAmount = totalAmount;
        }
    }
}