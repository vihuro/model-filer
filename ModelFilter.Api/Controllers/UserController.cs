using Microsoft.AspNetCore.Mvc;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Utils;
using ModelFilter.Domain.Utils.Filters;
using Newtonsoft.Json;

namespace ModelFilter.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers(string? filters)
        {
            try
            {
                var result = await _userRepository.GetAsync(JsonConvert.DeserializeObject<FilterBaseDto>(filters));
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
