
namespace TranThiThaoNhu_2124110075.API.DTOs.Orders
{
    public class OrderListItemDto
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public int ItemCount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
