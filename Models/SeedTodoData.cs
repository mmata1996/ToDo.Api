namespace ToDo.Api.Models
{
    public class SeedTodoData(IList<TodoItem> todoItems)
    {
        private IList<TodoItem> TodoItems { get; } = todoItems;
        
        public IList<TodoItem> Get() => TodoItems;
        
        public TodoItem? Get(long id) => TodoItems.FirstOrDefault(t => t.Id == id);
       
        public bool Add(TodoItem todoItem)
        {
            if (todoItem == null)
                return false;

            var nextId = TodoItems.Count == 0 ? 1 : TodoItems.Max(t => t.Id) + 1;

            todoItem.Id = nextId;
            todoItem.CreatedAt = DateTime.UtcNow;

            todoItems.Add(todoItem);
            return true;
        }

        public bool Update(long id, TodoItem todoItem)
        {
            if (todoItem == null)
                return false;

            var existingTodoItem = Get(id);

            if (existingTodoItem == null)
                return false;

            existingTodoItem.Title = todoItem.Title;
            existingTodoItem.Description = todoItem.Description;
            existingTodoItem.IsCompleted = todoItem.IsCompleted;

            if (existingTodoItem.IsCompleted)
            {
                existingTodoItem.CompletedAt = DateTime.Now;
            }
            else
            {
                existingTodoItem.CompletedAt = null;
            }

            return true;
        }
        public bool Delete(long id)
        {
            var todoItem = Get(id);
            if (todoItem == null)
                return false;
            todoItems.Remove(todoItem);
            return true;
        }
    }
}
