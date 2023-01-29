namespace OperationStacked.Response
{
    public sealed record LoginRequestResult(bool success, string error = "", string token = null, int userId = 0, int currentDay = 0, int currentWeek = 0, int workoutsInWeek = 0, string username = "");
}
