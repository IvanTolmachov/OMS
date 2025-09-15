namespace OMS.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public List<OrderItem> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
    }
}
