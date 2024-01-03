namespace OperationStacked.Requests
{
    public class CreateUser
    {
        public Guid CognitoUserId { get; set; }
        public string UserName { get; set; }
    }

}
