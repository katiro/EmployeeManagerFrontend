using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManager.Models;
using EmployeeManager.Models.Entity;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EmployeeManager.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string _EmployeeManagerApiUrl = "http://localhost:5017/";
    private string _Email = "userTest@testmail.com";
    private string _Password = "Pass@test01";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #region "Endpoint login"

    public Token Login(string email, string password)
    {
        Token token;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_EmployeeManagerApiUrl);

                var login = new Login
                {
                    Email = email,
                    Password = password
                };

                var postTask = client.PostAsJsonAsync<Login>("login", login);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    token = JsonSerializer.Deserialize<Token>(readTask.Result);
                }
                else
                {
                    token = null;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar sesión");
            token = null;
        }

        return token;
        
    }

    #endregion


    #region "Endpoints para la gestión de empleados"

    /// <summary>
    /// Obtener todos los empleados
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    public JsonResult GetEmployees()
    {
        JsonResult jsonResponse;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_EmployeeManagerApiUrl);

                //Agregar token a la petición
                Token token = Login(_Email, _Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var responseTask = client.GetAsync("api/Empleados");

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    jsonResponse = Json(new { employees = JsonSerializer.Deserialize<List<Empleado>>(readTask.Result), msj = "" });
                }
                else
                {
                    jsonResponse = Json(new { employees = "", msj = "Error al obtener los empleados" });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los empleados");
            jsonResponse = Json(new { employees = "", msj = "Error al obtener los empleados" });
        }


        return jsonResponse;
    }

    /// <summary>
    /// Crear un empleado
    /// </summary>
    /// <param name="employee"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    public JsonResult CreateEmployee(Empleado employee)
    {
        JsonResult jsonResponse;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_EmployeeManagerApiUrl);

                //Agregar token a la petición

                Token token = Login(_Email, _Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var postTask = client.PostAsJsonAsync<Empleado>("api/Empleados", employee);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    jsonResponse = Json(new { msj = "El empleado fue creado con exito" });
                }
                else
                {
                    jsonResponse = Json(new { msj = "Error al crear el empleado" });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el empleado");
            jsonResponse = Json(new { msj = "Error al crear el empleado" });
        }

        return jsonResponse;
    }

    /// <summary>
    /// Actualizar un empleado
    /// </summary>
    /// <param name="employee"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public JsonResult UpdateEmployee(int id, Empleado employee)
    {
        JsonResult jsonResponse;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_EmployeeManagerApiUrl);

                //Agregar token a la petición
                Token token = Login(_Email, _Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var putTask = client.PutAsJsonAsync<Empleado>("api/Empleados/" + id , employee);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    jsonResponse = Json(new { msj = "Se ha actualizado el empleado con exito" });
                }
                else
                {
                    jsonResponse = Json(new { msj = "Error al actualizar el empleado" });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el empleado");
            jsonResponse = Json(new { msj = "Error al actualizar el empleado" });
        }

        return jsonResponse;
    }

    /// <summary>
    /// Eliminar un empleado
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public JsonResult DeleteEmployee(int id)
    {
        JsonResult jsonResponse;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_EmployeeManagerApiUrl);

                //Agregar token a la petición
                Token token = Login(_Email, _Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var deleteTask = client.DeleteAsync("api/Empleados" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    jsonResponse = Json(new { msj = "Se ha eliminado el empleado con exito" });
                }
                else
                {
                    jsonResponse = Json(new { msj = "Error al eliminar el empleado" });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el empleado");
            jsonResponse = Json(new { msj = "Error al eliminar el empleado" });
        }

        return jsonResponse;
    }

    #endregion


}
