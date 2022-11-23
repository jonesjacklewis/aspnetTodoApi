using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemController : ControllerBase
    {
         private static readonly List<TodoItem> TodoItems = new() { 
            new TodoItem{
                Id = 1,
                Created = DateTime.Now,
                Title = "Walk Dog",
                IsComplete = false
            },
            new TodoItem
            {
                Id = 2,
                Created = DateTime.Now.AddDays(-1),
                Title = "Cook Food",
                IsComplete = true
            },
            new TodoItem
            {
                Id = 3,
                Created = DateTime.Now.AddDays(-2),
                Title = "Study",
                IsComplete = false
            }
        };

        private readonly ILogger<TodoItemController> _logger;

        public TodoItemController(ILogger<TodoItemController> logger)
        {
            _logger = logger;
        }

        // Post/Create/Add New Todo Item
        [HttpPost(Name = "AddNewTodoItem")]
        public void Post(TodoItemDTO todoItem)
        {
            IEnumerable<int> ids = TodoItems.Select(item => item.Id);
            int currentId = ids.Max();
            int nextId = currentId + 1;

            try
            {
                TodoItem newItem = new()
                {
                    Id = nextId,
                    Title = todoItem.Title,
                    IsComplete = todoItem.IsComplete,
                    Created = DateTime.Parse(todoItem.Created ?? "")
                };

                TodoItems.Add(newItem);
            }
            catch
            {
                return;
            }

        }

        // Get/Retrieve All Todo Items
        [HttpGet(Name = "GetAllTodoItems")]
        public IEnumerable<TodoItem> Get()
        {
            return TodoItems.ToArray();
        }

        // Get/Retrieve a Todo Item
        [HttpGet("{id}", Name = "GetTodoItem")]
        public TodoItem? Get(int id)
        {
            return TodoItems.FirstOrDefault(x => x?.Id == id, null);
        }

        // Delete all Todo Items

        [HttpDelete(Name = "DeleteAllTodoItems")]
        public void Delete()
        {
            TodoItems.Clear();
        }

        // Delete a Todo Item
        [HttpDelete("{id}", Name = "DeleteTodoItem")]
        public void Delete(int id)
        {
            TodoItems.Remove(TodoItems.First(x => x.Id == id));
        }
    }
}