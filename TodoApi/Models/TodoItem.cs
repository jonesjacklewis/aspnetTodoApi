namespace TodoApi.Models
{
    public class TodoItem : TodoItemDTO
    {
        public int Id { get; set; }
        public new DateTime Created { get; set; }

    }
}
