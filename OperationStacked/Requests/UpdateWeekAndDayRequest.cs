namespace OperationStacked.Requests
{
    public class UpdateWeekAndDayRequest
    {
        public int Week { get; set; }
        public int Day { get; set; }
        public Guid UserId { get; set; }
    }
}
