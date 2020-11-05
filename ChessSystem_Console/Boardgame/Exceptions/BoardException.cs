using System;

namespace Boardgame
{
    public class BoardException : ApplicationException
    {
        public BoardException(string message) : base(message)
        {
        }
    }
}