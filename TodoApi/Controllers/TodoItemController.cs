using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

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
        private readonly DataContext _dataContext;

        public TodoItemController(ILogger<TodoItemController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }


#pragma warning disable S1135 // Track uses of "TODO" tags - not sensible in this project
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
                _dataContext.TodoItems.Add(newItem);
                _dataContext.SaveChanges();
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

        // Partial Patch/Update a Todo Item
        [HttpPatch("{id}",Name = "PatchTodoItem")]
        public void Patch(int id, TodoItemDTO updatedItem)
        {
            try
            {
                TodoItem? currentItem = TodoItems.FirstOrDefault(x => x.Id == id);

                if (currentItem == null) { return; }

                TodoItems.Remove(currentItem);


                if (currentItem.Created != DateTime.Parse(updatedItem.Created ?? ""))
                {
                    currentItem.Created = DateTime.Parse(updatedItem.Created ?? "");
                }

                if(currentItem.Title != updatedItem.Title)
                {
                    currentItem.Title = updatedItem.Title;
                }

                if(currentItem.IsComplete != updatedItem.IsComplete)
                {
                    currentItem.IsComplete = updatedItem.IsComplete;
                }

                TodoItems.Add(currentItem);

            }
            catch
            {
                // do nothing
            }
           
        }

        // Complete Put/Update of a Todo Item
        [HttpPut("{id}", Name = "PatchTodoItem")]
        public void Put(int id, TodoItemDTO updatedItem)
        {
            try
            {
                TodoItem? currentItem = TodoItems.FirstOrDefault(x => x.Id == id);

                if (currentItem == null) { return; }

                TodoItems.Remove(currentItem);

                TodoItem updated = new()
                {
                    Id = id,
                    Created = DateTime.Parse(updatedItem.Created ?? ""),
                    Title = updatedItem.Title,
                    IsComplete = updatedItem.IsComplete
                };

                TodoItems.Add(updated);


            }
            catch
            {
                return;
            }
        }

        // Delete all Todo Items

        [HttpDelete(Name = "DeleteAllTodoItems")]
        public void Delete()
        {
            TodoItems.Clear();
        }

        // Delete a Todo Item
#pragma warning restore S1135
        [HttpDelete("{id}", Name = "DeleteTodoItem")]
        public void Delete(int id)
        {
            TodoItems.Remove(TodoItems.First(x => x.Id == id));
        }
    }
}