namespace API.DTOs
{
    public class UpdateStockDto
    {
        public int Quantity { get; set; }
    }

    public class StockUpdateResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int NewQuantity { get; set; }
        public string Message { get; set; }
    }
}