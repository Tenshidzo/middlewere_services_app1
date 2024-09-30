using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using services_app.Models;
using services_app.Services;

namespace services_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IServiceUsers? _serviceUsers;
        public HomeController(IServiceUsers? serviceUsers, UserContext? context)
        {
            _serviceUsers = serviceUsers;
            _serviceUsers.Context = context;
        }
        //GET: https://localhost:[port]/api/home
        [HttpGet]
        public JsonResult Get()
            => Json(_serviceUsers?.Read());
        //GET: https://localhost:[port]/api/home/[id]
        [HttpGet("{id}")]
        public JsonResult GetUser(int id)
            => Json(_serviceUsers?.GetUserById(id));
        [HttpPost]
        public JsonResult PostUser(User user)
            => Json(_serviceUsers?.Create(user));
        [HttpPut]
        public JsonResult PutUser(User user)
            => Json(_serviceUsers?.Update(user));
        [HttpDelete("{id}")]
        public bool DeleteUser(int id)
            => _serviceUsers.Delete(id);
    }
}
