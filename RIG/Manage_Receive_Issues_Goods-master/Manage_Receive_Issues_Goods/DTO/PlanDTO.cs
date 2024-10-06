namespace Manage_Receive_Issues_Goods.DTO
{
    public class PlanDTO
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public int TotalShipment { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public List<PlanDetailDTO> PlanDetails { get; set; } = new List<PlanDetailDTO>();
    }
}
