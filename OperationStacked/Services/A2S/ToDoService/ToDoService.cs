﻿using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Services.A2S.ToDoService
{
    public class ToDoService : IToDoService
    {
        public OperationStackedContext _context { get; set; }
        public ToDoService(OperationStackedContext context)
        {
            _context = context;
        }
        public async Task CreateToDo(CreateToDoRequest request)
        {
            try
            {
                var toDo = new ToDo()
                {
                    Title = request.Title,
                    Description = request.Description,
                    Username = request.Username,
                    Completed = false,
                };

                await _context.ToDos.AddAsync(toDo);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ThrowHelper.ThrowArgumentNullException("Error:");
            }
        }
    }
}


