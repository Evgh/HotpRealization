using HotpServer.Contracts.Responces;
using HotpServer.Storage;
using HotpServer.Storage.Implementations;
using HotpServer.Storage.Models;
using HotpServer.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HotpServer.Controllers
{

#if DEBUG

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        ApplicationContext _applicationContext;
        IDataLayer _dataLayer;

        public TestController(ApplicationContext applicationContext, 
                              IDataLayer dataLayer) : base()
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            _dataLayer = dataLayer ?? throw new ArgumentNullException(nameof(dataLayer));
        }

        [HttpGet("All")]
        public async Task<ActionResult<IList<User>>> GetAll()
        {
            return Ok(_applicationContext.Users.ToList());
        }

        [HttpGet("UserById/{id}")]
        public async Task<ActionResult<User>> GetUserById([FromRoute] int id)
        {
            User user = await _dataLayer.GetUserByIdAsync(id);

            if (user != null)
                return Ok(user);
                
            return NotFound();
        }

        [HttpGet("UserByLogin/{login}")]
        public async Task<ActionResult<User>> GetUserByLogin([FromRoute] string login)
        {
            User user = await _dataLayer.GetUserByLoginAsync(login);

            if (user != null)
                return Ok(user);
            
            return NotFound();
        }

        [HttpGet("TestMapping")]
        public async Task<ActionResult<UserResponce>> MapUser()
        {
            User user = await _dataLayer.GetUserByIdAsync(1);

            if (user != null)
                return Ok(MapperBuilder.CreateMapper<User, UserResponce>().Map<User, UserResponce>(user));

            return NotFound();
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            await _dataLayer.AddOrUpdateUserAsync(user);

            return Ok();
        }
    }

#endif
}
