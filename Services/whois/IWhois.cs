using System.Threading.Tasks;

namespace ProjetoRedehost.Services.whois
{
    public interface IWhois
    {
         Task<string> WhoisAsync(string domain);

    }
}