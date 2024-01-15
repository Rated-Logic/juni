using System.Collections.Generic;

namespace todolist.Models.ViewModels{
    public class TodoViewModel{
        public List<TodoItem> Todolis {get; set; }
        public TodoItem Todo {get; set;}
    }
}