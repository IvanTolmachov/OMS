namespace OMS.Api.Models
{
    public record OrderDto(int Id, DateTimeOffset Timestamp, List<OrderItemDto> Items, decimal TotalAmount);
}