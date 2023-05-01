namespace OperationStacked.Requests
{
    public class CreateUser
    {
        public string CognitoUserId { get; set; }
        public string UserName { get; set; }
        public int WorkoutDaysInweek { get; set; }
    }

}
