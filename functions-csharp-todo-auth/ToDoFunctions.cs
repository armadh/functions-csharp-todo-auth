using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Todo.Models;

namespace Todo
{
    public class ToDoFunctions
    {
        private readonly IStore _store;
        public ToDoFunctions(IStore store)
        {
            _store = store;
        }

        [FunctionName("GetTodos")]
        public async Task<IActionResult> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            var userId = req.HttpContext.User.Identity.Name ?? "test";
            log.LogInformation($"GET Todos triggered for {userId}.");

            return new OkObjectResult(await _store.GetItemsAsync(userId));
        }

        [FunctionName("PutTodo")]
        public async Task<IActionResult> PutTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            var userId = req.HttpContext.User.Identity.Name ?? "test";
            log.LogInformation($"PUT Todos triggered for {userId}.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var todo = JsonConvert.DeserializeObject<ToDoItem>(requestBody);
            todo.UserId = userId;

            log.LogInformation($"Have todo item: {JsonConvert.SerializeObject(todo)}");

            await _store.PutItemAsync(todo);

            return new OkResult();
        }

        [FunctionName("DeleteTodo")]
        public async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            var userId = req.HttpContext.User.Identity.Name ?? "test";
            log.LogInformation($"DELETE Todos triggered for {userId}.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var todo = JsonConvert.DeserializeObject<ToDoItem>(requestBody);
            todo.UserId = userId;

            log.LogInformation($"Have todo item: {JsonConvert.SerializeObject(todo)}");

            await _store.DeleteItemAsync(todo);

            return new OkResult();
        }

        [FunctionName("UpdateTodo")]
        public async Task<IActionResult> PostTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            var userId = req.HttpContext.User.Identity.Name ?? "test";
            log.LogInformation($"POST Todos triggered for {userId}.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var todo = JsonConvert.DeserializeObject<ToDoItem>(requestBody);
            todo.UserId = userId;

            log.LogInformation($"Have todo item: {JsonConvert.SerializeObject(todo)}");

            await _store.UpdateItemAsync(todo);

            return new OkResult();
        }
    }
}
