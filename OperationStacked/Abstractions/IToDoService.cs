using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Abstractions
{
    public interface IToDoService
    {
        Task CreateToDo(CreateToDoRequest request);
    }
}
