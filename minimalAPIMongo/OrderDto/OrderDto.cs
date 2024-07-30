namespace minimalAPIMongo.OrderDto
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; } 
        public string ProductId { get; set; }
        public string ProductName { get; set; } 
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, string> AdditionalAttributes { get; set; }
    }
}
