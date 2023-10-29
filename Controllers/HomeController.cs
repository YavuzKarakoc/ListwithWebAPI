using EtrWebAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EtrWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetTokenAsync([FromBody] User user)
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user.username}:{user.password}"));
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient(clientHandler);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://efatura.etrsoft.com/fmi/data/v1/databases/testdb/sessions");
            request.Headers.Add("Authorization", "Basic "+ credentials);
            var content = new StringContent("{}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }

        [HttpPatch("GetList")]
        public async Task<IActionResult> GetDebtList([FromBody] Token token)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient(clientHandler);
            var request = new HttpRequestMessage(HttpMethod.Patch, "https://efatura.etrsoft.com/fmi/data/v1/databases/testdb/layouts/testdb/records/1");
            request.Headers.Add("Authorization", "Bearer "+token.token);
            var content = new StringContent("{\"fieldData\": {\r\n    \r\n    },\r\n  \"script\" : \"getData\"\r\n}\r\n", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result =await response.Content.ReadAsStringAsync();
            return Ok(result);
        }
    }
}
