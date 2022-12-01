namespace TodoApi.Models
{
    public class TodoItemDTO
    {
        public string? Title { get; set; }
        public string? Created { get; set; }
        public bool IsComplete { get; set; }
    }
}
