using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Response
{
    public sealed record RegisterRequestResult(bool success, string error = "", int userId = 0);
}
