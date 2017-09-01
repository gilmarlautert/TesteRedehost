using System;

namespace ProjetoRedehost.Model
{
    public class RequestResult
    {
        public RequestState State { get; set; }
        public string Msg { get; set; }
        public Object Data { get; set; }
    }
}