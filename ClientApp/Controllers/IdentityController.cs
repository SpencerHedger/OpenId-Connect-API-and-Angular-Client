using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ClientApp.Controllers
{
    public class IdentityController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();

            // Discover endpoints from metadata.
            var oidcDiscoveryResult = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (oidcDiscoveryResult.IsError)
            {
                Console.WriteLine(oidcDiscoveryResult.Error);
                return Json(oidcDiscoveryResult.Error);
            }

            // Request token.
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = oidcDiscoveryResult.TokenEndpoint,

                ClientId = "clientApp",
                ClientSecret = "secret",
                Scope = "resourceApi"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                throw new HttpRequestException(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // Call the resource API with the access token obtained from IdentityProvider.
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new HttpRequestException(response.StatusCode.ToString());
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
                return Json(content);
            }
        }
    }
}
