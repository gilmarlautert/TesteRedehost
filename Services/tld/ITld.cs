using ProjetoRedehost.Models;
using System.Collections.Generic;
using ProjetoRedehost.ViewModels;

namespace ProjetoRedehost.Services.tld
{
    public interface ITld
    {
         Tld Add(Tld tld);
         Tld Find(int id);
         void Edit(Tld tld);
         void Remove(int id);
         IEnumerable<Tld> ListAll();
    }
}