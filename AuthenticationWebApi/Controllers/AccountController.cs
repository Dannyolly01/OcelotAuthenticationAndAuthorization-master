
using Common.Models;
using Grpc.Net.Client;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        private readonly HttpClient _httpClient;
        public AccountController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(3)
            };
            _httpClient = new HttpClient();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest authenticationRequest) 
        {
            var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(authenticationRequest);
            
            if (authenticationResponse == null) return Unauthorized();
            try
            {
                //var httpResponseMessage = await _httpClient.GetAsync("http://localhost:5284/api/customer");
                //var deserializeObject = JsonConvert.DeserializeObject<List<CustomerA>>(httpResponseMessage.Content.ReadAsStringAsync().Result);
                return Ok(new
                {
                    AuthRes = authenticationResponse,
                    //CustomerRes = deserializeObject
                });
    
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
