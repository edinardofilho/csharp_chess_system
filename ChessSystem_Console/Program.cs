using Boardgame;
using Chess;
using System;

namespace ChessSystem_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessPosition position = new ChessPosition('c', 7);

            Console.WriteLine(position);
            Console.WriteLine(position.ToPosition());

            Console.ReadLine();

        }
    }
}
