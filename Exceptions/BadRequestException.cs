using System;

namespace ProjetoRedehost.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {           
        }
    }
}  