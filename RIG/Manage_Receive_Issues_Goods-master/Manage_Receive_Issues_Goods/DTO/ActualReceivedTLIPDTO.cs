namespace Manage_Receive_Issues_Goods.DTO
{
    public class ActualReceivedTLIPDTO
    {
        public int ActualReceivedId { get; set; }
        public DateTime ActualDeliveryTime { get; set; }
        public TimeOnly? ActualLeadTime { get; set; }
        public string SupplierCode { get; set; }
		public string? AsnNumber { get; set; }
		public string? DoNumber { get; set; }
        public string? Invoice { get; set; }
        public string SupplierName { get; set; }
        public List<ActualDetailTLIPDTO> ActualDetails { get; set; }
    }
}
