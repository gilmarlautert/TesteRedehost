using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjetoRedehost.Services.whois
{
    public class WhoisService : IWhois
    {
        private WhoisStruct _uriSearch;
        public WhoisService(WhoisStruct uriSearch)
        {
            _uriSearch = uriSearch;
        }

        public async Task<string> WhoisAsync(string domain)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            Task<string> jsonRetornoTask = client.GetStringAsync($"{_uriSearch.URI}?{_uriSearch.Param}={domain}");
            string jsonRetorno = await jsonRetornoTask; 
            return jsonRetorno;
            
        }
    }
}