using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using todolist.Models;
using System.Web;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using todolist.Models.ViewModels;


namespace todolist.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var todolistViewModel = GetAllTodos();
        return View(todolistViewModel);
    }

    public JsonResult PopulateForm(int id){
        var todo = GetById(id);
        return Json(todo);
    }

    internal TodoViewModel GetAllTodos(){
        List<TodoItem> todolis = new();

        using (SqliteConnection con = 
        new SqliteConnection("Data Source=db.sqlite")){

            using (var tableCmd = con.CreateCommand()){
                con.Open();
                tableCmd.CommandText = "SELECT * FROM todo";

                using (var reader = tableCmd.ExecuteReader()){
                    if(reader.HasRows){
                        while(reader.Read()){
                            todolis.Add(
                                new TodoItem
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                });
                        }
                    }
                    else{
                        return new TodoViewModel{
                            Todolis = todolis
                        };
                    }
                }
            }
        }
        return new TodoViewModel
    {
        Todolis = todolis
    };
    }

    internal TodoItem GetById(int id){
        TodoItem todo = new();

        using (var connection =
        new SqliteConnection("Data Source = db.sqlite")){
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $"SELECT * FROM todo where Id = '{id}'";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        reader.Read();
                        todo.Id = reader.GetInt32(0);
                        todo.Name = reader.GetString(1);


                    }
                    else{
                        return todo;
                    }
                }
            }
        }
        return todo;
    }

    public RedirectResult Insert(TodoItem todo){
        using(SqliteConnection con = 
        new SqliteConnection("Data Source = db.sqlite")){

            using (var tableCmd = con.CreateCommand()){
                con.Open();
                tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex){
                    Console.WriteLine(ex.Message);
                }
            }
            
        }
        return Redirect("http://localhost:5066/");
    }

   public JsonResult Delete(int id){
        using (SqliteConnection con = 
        new SqliteConnection("Data Source =db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
                tableCmd.ExecuteNonQuery();
            }
        }

        return Json(new {});
   }

   public RedirectResult Update(TodoItem todo){
     using (SqliteConnection con =
     new SqliteConnection("Data Source=db.sqlite"))
     {
         using (var tableCmd = con.CreateCommand())
         {
             con.Open();
             tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
             try
             {
                 tableCmd.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
         }
     }
     return Redirect("http://localhost:5066");
  } 
}

