using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Abstractions;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Services.UserAccountsService;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace OperationStacked.Controllers
{
    [ApiController]
    [DisplayName("Workout Generation")]
    [Route("user/")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly IToDoRepository _toDoRepository;

        public ToDoController(IToDoService toDoService,IToDoRepository toDoRepository)
        {
            _toDoService = toDoService;
            _toDoRepository = toDoRepository;
        }

        [Route("create-to-do")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoRequest request)
        {
            await _toDoService.CreateToDo(request);
            return Ok();

        }

        [Route("complete-to-do")]
        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            await  _toDoRepository.CompleteToDo(id);
            return Ok();
        }
        [Route("get-to-do-list")]
        [HttpPost]
        public async Task<IActionResult> Get(string username)
        {
            var result = await _toDoRepository.GetToDosForUser(username);
            return Ok(result);
        }
    }
}