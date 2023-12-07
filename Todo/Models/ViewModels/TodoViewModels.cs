namespace Todo.Models.ViewModel
{
    public class TodoViewModel {
        public required List<TodoModel> TodoList { get; set; }
        public TodoModel Todo { get; set; }
    }
}