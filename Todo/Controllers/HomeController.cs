using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Todo.Models;
using Todo.Models.ViewModel;

namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var todoListViewModel = GetAllTodos();
        return View(todoListViewModel);
    }

    internal TodoViewModel GetAllTodos() {
        List<TodoModel> todoList = new();
        using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite")) {
            using (var tableCommand = con.CreateCommand()) {
                con.Open();
                tableCommand.CommandText = "SELECT * FROM todo";
                using (var reader = tableCommand.ExecuteReader()){
                    if (reader.HasRows) {
                        while (reader.Read()) {
                            todoList.Add(new TodoModel{
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                    return new TodoViewModel {
                        TodoList = todoList
                    };
                }
            }
        }
    }

    public RedirectResult Insert(TodoModel todo) 
    {
        using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite")) 
        {
            using (var tableCommand = con.CreateCommand()) 
            {
                con.Open();
                tableCommand.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
                try {
                    tableCommand.ExecuteNonQuery();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }
        return Redirect("/");
    }

    [HttpPost]
    public JsonResult Delete(int id) {
        using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite")){
            using(var tableCommand = con.CreateCommand()){
                con.Open();
                tableCommand.CommandText = $"DELETE FROM todo WHERE Id = '{id}'";
                tableCommand.ExecuteNonQuery();
            }
        }
        return Json(new{});
    }

    [HttpGet]
    public JsonResult PopulateForm(int id) {
        var todo = GetById(id);
        return Json(todo);
    }

    internal TodoModel GetById(int id)
    {
        using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite")) {
            TodoModel todo = new();
            using (var tableCommand = con.CreateCommand()) {
                con.Open();
                tableCommand.CommandText = $"SELECT * FROM todo WHERE Id = '{id}'";
                using (var reader = tableCommand.ExecuteReader()){
                    if (reader.HasRows) {
                        while (reader.Read()) {
                            todo.Id = reader.GetInt32(0);
                            todo.Name = reader.GetString(1);
                        }
                    }
                    return todo;
                }
            }
        }
    }

    public RedirectResult Update(TodoModel todo) {
        using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite")) 
        {
            using (var tableCommand = con.CreateCommand()) 
            {
                con.Open();
                tableCommand.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
                try {
                    tableCommand.ExecuteNonQuery();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }
        return Redirect("/");
    }
}
