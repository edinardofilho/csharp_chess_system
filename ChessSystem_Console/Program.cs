using Boardgame;
using System;

namespace ChessSystem_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Position p = new Position(3, 4);

            System.Console.WriteLine("Position: " + p);

            Console.ReadLine();
        }
    }
}
