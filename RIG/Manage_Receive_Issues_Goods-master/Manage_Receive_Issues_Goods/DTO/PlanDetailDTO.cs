namespace Manage_Receive_Issues_Goods.DTO
{
    public class PlanDetailDTO
    {
        public int PlanDetailId { get; set; }
        public TimeOnly PlanTime { get; set; }
        public string PlanDetailName { get; set; }
        public List<ActualDetailDTO> Actuals { get; set; } = new List<ActualDetailDTO>();
    }
}
