using HotpServer.Models.Dto;
using HotpServer.Services;
using HotpServer.Storage;
using HotpServer.Storage.Implementations;
using HotpServer.Storage.Models;
using HotpServer.Utilities;
using Microsoft.AspNetCore.Http;
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
            _applicationContext = applicationContext;
            _dataLayer = dataLayer;
        }

        [HttpGet("GetAll")]
        public async Task<IList<User>> Get()
        {
            return _applicationContext.Users.ToList();
        }

        [HttpGet("UserById")]
        public async Task<User> Get([FromQuery]int id)
        {
            return await _dataLayer.GetUserByIdAsync(id);
        }

        [HttpGet("UserByLogin")]
        public async Task<User> Get([FromQuery] string login)
        {
            return await _dataLayer.GetUserByLoginAsync(login);
        }

        [HttpGet("MapUser")]
        public async Task<UserDto> MapUser()
        {
            User user = await _dataLayer.GetUserByIdAsync(1);

            if (user != null)
            {
                return MapperBuilder.CreateMapper<User, UserDto>().Map<User, UserDto>(user);
            }
            return null;
        }

        [HttpPost]
        public async Task Task([FromBody] User user)
        {
            await _dataLayer.AddOrUpdateUserAsync(user);
        }
    }

#endif
}
