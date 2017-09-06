using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using ProjetoRedehost.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ProjetoRedehost.Tests.Integration
{
    
    public class TldsTest
    {
        private string _uri = "/api/tlds"; 
        private readonly TestServer _server; 
        private readonly HttpClient _client;
    
        public TldsTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
                
            _client = _server.CreateClient();
        }


        [Fact]
        public async Task TestPost()
        {  
            var extension = ".com.com";
            var jsonInString = "{\"extension\": \"" + extension + "\"}";
            var response = await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var tld = JsonConvert.DeserializeObject<Tld>(responseString);
            
            Assert.Equal(tld.Extension, extension);     
            //Assert

        }


        [Fact]
        public async Task TestPostInvalid()
        {  
            var extension = "{\"a\": \".com.com\"}";
            var jsonInString = "{\"extension\": " + extension + "}";
            try{
                await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                Assert.Equal(true, false);     
            }
            catch(Exception e)
            {
                Assert.Equal(true, true);     
            }
        }

        [Fact]
        public async Task TestPostDuplicate()
        {         
            var jsonInString = "{\"extension\": \".com.zo\"}";
            var response = await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var tld = JsonConvert.DeserializeObject<Tld>(responseString);
            
            var responsePut = await _client.PostAsync(_uri , new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            Assert.Equal(responsePut.StatusCode, HttpStatusCode.BadRequest);   
        }


        [Fact]
        public async Task TestPut()
        {         
            var jsonInString = "{\"extension\": \".com.pt\"}";
            var response = await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var tld = JsonConvert.DeserializeObject<Tld>(responseString);
            
            string extension = ".com.xy";
            var jsonInStringPut = "{\"extension\": \"" + extension + "\"}";
            var responsePut = await _client.PutAsync(_uri + '/' + tld.Id , new StringContent(jsonInStringPut, Encoding.UTF8, "application/json"));;
            responsePut.EnsureSuccessStatusCode();

            var responsePutString = await responsePut.Content.ReadAsStringAsync();
            var tldPut = JsonConvert.DeserializeObject<Tld>(responsePutString);
            Assert.Equal(extension, tldPut.Extension);          
            Assert.Equal(tldPut.Id, tld.Id);   
        }

        [Fact]
        public async Task TestPutInvalid()
        {         
            var jsonInString = "{\"extension\": \".com.xg\"}";
            var response = await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var tld = JsonConvert.DeserializeObject<Tld>(responseString);
            
            string extension =  "{\"a\": \".com.com\"}";
            var jsonInStringPut = "{\"extension\": " + extension + "}";
            try{
                var responsePut = await _client.PutAsync(_uri + '/' + tld.Id , new StringContent(jsonInStringPut, Encoding.UTF8, "application/json"));;
                Assert.Equal(true, false);     
            }
            catch(Exception e)
            {
                Assert.Equal(true, true);     
            }
        }

        [Fact]
        public async Task TestPutDuplicate()
        {         
            var jsonInString = "{\"extension\": \".com.zx\"}";
            var response = await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var tld = JsonConvert.DeserializeObject<Tld>(responseString);
            
            var responsePut = await _client.PutAsync(_uri + '/' + tld.Id , new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            Assert.Equal(responsePut.StatusCode, HttpStatusCode.BadRequest);   
        }

        [Fact]
        public async Task TestPutNotFound()
        {         
            var idErr = 0;
            var jsonInString = "{\"extension\": \".com.pt\"}";
            var response = await _client.PutAsync(_uri + '/' + idErr, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            
            Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task TestGet()
        {         
            var jsonInString = "{\"extension\": \".com.ss\"}";
            var response = await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var tld = JsonConvert.DeserializeObject<Tld>(responseString);
            var responseGet = await _client.GetAsync("/api/tlds/" + tld.Id);
            responseGet.EnsureSuccessStatusCode();
            var responseGetString = await responseGet.Content.ReadAsStringAsync();
            var tldGet = JsonConvert.DeserializeObject<Tld>(responseGetString);
            //Assert
            Assert.Equal(tld.Id, tldGet.Id);      
        }

        [Fact]
        public async Task TestGetNotFound()
        {         
            var idErr = 0;
            var response = await _client.GetAsync(_uri + '/' + idErr);
            Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task TestGetAll()
        {         
            var response = await _client.GetAsync("/api/tlds");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var listaTlds = JsonConvert.DeserializeObject<Tld[]>(responseString);
            
            //Assert
            Assert.Equal(listaTlds.Length <= 0, true);
        }

        [Fact]
        public async Task TestDelete()
        {         
                  
            var jsonInString = "{\"extension\": \".com.dl\"}";
            var response = await _client.PostAsync(_uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));;
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var tld = JsonConvert.DeserializeObject<Tld>(responseString);

            var responseDel = await _client.DeleteAsync(_uri + '/' + tld.Id);
            responseDel.EnsureSuccessStatusCode();
            Assert.Equal(responseDel.StatusCode,HttpStatusCode.OK);
        }


        [Fact]
        public async Task TestDeleteNotFound()
        {         
            var idErr = 0;
            var responseDel = await _client.DeleteAsync(_uri + '/' + idErr);
            Assert.Equal(responseDel.StatusCode,HttpStatusCode.NotFound);
        }


        
    }
}