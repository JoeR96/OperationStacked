namespace OperationStacked.Response
{
    public sealed record LoginRequestResult(bool success, string error = "", string V = null);
}
