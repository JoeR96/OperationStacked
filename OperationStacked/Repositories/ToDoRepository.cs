using OperationStacked.Data;
using OperationStacked.Entities;

namespace OperationStacked.Repositories
{
    public class ToDoRepsitory : IToDoRepository
    {
        private readonly OperationStackedContext _context;

        public ToDoRepsitory(OperationStackedContext context)
        {
            _context = context;
        }

        public async Task InsertToDo(ToDo toDo)
        {
            toDo.CreatedDate = DateTime.Now;
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task CompleteToDo(int id)
        {
            var toDo = _context.ToDos.Where(x => x.Id == id).FirstOrDefault();
            toDo.Completed = true;
            toDo.CompletedDate = DateTime.Now;
            _context.ToDos.Update(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ToDo>> GetToDosForUser(string username)
            => _context.ToDos.Where(x => x.Username == username).ToList();

    }
}
