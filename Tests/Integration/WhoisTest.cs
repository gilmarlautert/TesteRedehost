using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace ProjetoRedehost.Tests.Integration
{
    public class WhoisTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
    
        public WhoisTest()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task TestSuccess()
        {         
            var extension = ".com.br";
            var response = await _client.GetAsync("/api/whois?text=" + extension);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains(extension,
                responseString);          

        }

        [Fact]
        public async Task TestFail()
        {         
            var extension = ".com";
            var response = await _client.GetAsync("/api/whois?text=" + extension);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.DoesNotContain(extension,
                responseString);          

        }
        
    }
}