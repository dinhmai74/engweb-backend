using HollypocketBackend.DTO;
using HollypocketBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollypocketBackend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context) {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems() {
            return await _context.TodoItems.Where(x => !x.IsDelete).Select(x => ConvertItemToDTO(x)).ToListAsync();
            //Where().Select(x => ConvertItemToDTO(x)).ToListAsync();
            //return await _context.TodoItems.Select(x => ConvertItemToDTO(x)).ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id) {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null) {
                return NotFound();
            }

            return ConvertItemToDTO(todoItem);
        }

        // POST: api/TodoItems/create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("create")]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDto todoItemDto) {
            var todoItem = new TodoItem {
                IsComplete = todoItemDto.IsComplete,
                Name = todoItemDto.Name,
                IsDelete = false
            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ConvertItemToDTO(todoItem));
        }


        // update
        [HttpPost("update/{id}")]
        public async Task<ActionResult<TodoItemDto>> PutTodoItem(long id, TodoItemDto todoItemDto) {
            if (id != todoItemDto.Id) {
                return BadRequest();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null) {
                return NotFound();
            }

            todoItem.IsComplete = todoItemDto.IsComplete;
            todoItem.Name = todoItemDto.Name;

            if (
               await TryUpdateModelAsync<TodoItem>(
                  todoItem,
                  "",
                  s => s.Id)
           ) {
                try {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */) {
                    //Log the error (uncomment ex variable name and write a log.)
                    UpdateFailed("update ");
                }
            }

            return ConvertItemToDTO(todoItem);
        }

        private void UpdateFailed(string mess) {
            ModelState.AddModelError("", "Unable to " + mess +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
        }

        // DELETE: with post api/TodoItems/delete?id=5
        [HttpPost("delete")]
        public async Task<ActionResult<TodoItemDto>> DeleteTodoItem(long id) {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                return NotFound();
                
            }
            todoItem.IsDelete = true;

            if (
                await TryUpdateModelAsync<TodoItem>(
                   todoItem,
                   "",
                   s => s.Id)
            ) {
                try {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */) {
                    UpdateFailed("Delete.");
                }

            }
            return ConvertItemToDTO(todoItem);
        }

        private bool TodoItemExists(long id) {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private static TodoItemDto ConvertItemToDTO(TodoItem todoItem) =>
               new TodoItemDto {
                   Id = todoItem.Id,
                   Name = todoItem.Name,
                   IsComplete = todoItem.IsComplete
               };

    }
}
