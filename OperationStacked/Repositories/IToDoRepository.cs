using OperationStacked.Entities;

namespace OperationStacked.Repositories
{
    public interface IToDoRepository
    {
        public Task InsertToDo(ToDo toDo);
        public Task CompleteToDo(int id);
        public Task<List<ToDo>> GetToDosForUser(string username);

    }
}