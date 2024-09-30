using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace middlewere_services_app
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            List<User> users = new List<User>
            {
                new User{ Id = 1, Name = "Alex" },
                new User{ Id = 2, Name = "Bob" },
                new User{ Id = 3, Name = "Tom" },
                new User{ Id = 4, Name = "Sam" },
                new User{ Id = 5, Name = "Felix" }
            };

            // Middleware для обробки запитів
            app.Run(async (context) =>
            {
                var request = context.Request;
                var response = context.Response;
                string path = request.Path;
                response.Headers.Append("Content-Type", "application/json; charset=utf-8");

                if (path == "/")
                {
                    response.StatusCode = 200;
                    await response.SendFileAsync("wwwroot/Index.html");
                }
                else if (path.StartsWith("/users"))
                {
                    if (request.Method == HttpMethods.Get && request.Query.ContainsKey("id")) // GetUserById
                    {
                        int id = Int32.Parse(request.Query["id"]);
                        var user = users.Find(u => u.Id == id);

                        if (user != null)
                        {
                            response.StatusCode = 200;
                            await response.WriteAsJsonAsync(user);
                        }
                        else
                        {
                            response.StatusCode = 404;
                            await response.WriteAsync($"User with ID {id} not found.");
                        }
                    }
                    else if (request.Method == HttpMethods.Post) // Add user
                    {
                        if (request.Query.ContainsKey("id") && request.Query.ContainsKey("name"))
                        {
                            int id = Int32.Parse(request.Query["id"]);
                            string name = request.Query["name"];
                            users.Add(new User { Id = id, Name = name });
                            response.StatusCode = 201;
                            await response.WriteAsJsonAsync(new { message = "User added successfully!" });
                        }
                        else
                        {
                            response.StatusCode = 400;
                            await response.WriteAsync("Invalid parameters.");
                        }
                    }
                    else if (request.Method == HttpMethods.Put && request.Query.ContainsKey("id")) // Update user
                    {
                        int id = Int32.Parse(request.Query["id"]);
                        var user = users.Find(u => u.Id == id);

                        if (user != null)
                        {
                            if (request.Query.ContainsKey("name"))
                            {
                                user.Name = request.Query["name"];
                                response.StatusCode = 200;
                                await response.WriteAsJsonAsync(new { message = "User updated successfully!" });
                            }
                            else
                            {
                                response.StatusCode = 400;
                                await response.WriteAsync("Missing 'name' parameter.");
                            }
                        }
                        else
                        {
                            response.StatusCode = 404;
                            await response.WriteAsync($"User with ID {id} not found.");
                        }
                    }
                    else if (request.Method == HttpMethods.Delete && request.Query.ContainsKey("id")) // Delete user
                    {
                        int id = Int32.Parse(request.Query["id"]);
                        var user = users.Find(u => u.Id == id);

                        if (user != null)
                        {
                            users.Remove(user);
                            response.StatusCode = 200;
                            await response.WriteAsJsonAsync(new { message = "User deleted successfully!" });
                        }
                        else
                        {
                            response.StatusCode = 404;
                            await response.WriteAsync($"User with ID {id} not found.");
                        }
                    }
                    else // Get all users
                    {
                        response.StatusCode = 200;
                        await response.WriteAsJsonAsync(users);
                    }
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsync("Page not found.");
                }
            });

            app.Run();
        }
    }
}
