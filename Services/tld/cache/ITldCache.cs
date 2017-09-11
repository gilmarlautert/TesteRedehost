using System.Collections.Generic;

namespace ProjetoRedehost.Services.tld.cache
{
    public interface ITldCache
    {
         void Add(string extension);
         string Find(string extension);
         void Edit(string extensionOrigin, string extensionDestination);
         void Remove(string extension);
         IEnumerable<string> ListAll();
    }
}