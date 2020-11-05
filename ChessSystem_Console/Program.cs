using Boardgame;
using Chess;
using System;
using System.Collections.Concurrent;

namespace ChessSystem_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChessMatch chessMatch = new ChessMatch();

                while (!chessMatch.GameOver)
                {
                    Console.Clear();
                    UI.PrintBoard(chessMatch.Board);

                    Console.WriteLine(); 
                    Console.Write("Source: ");
                    Position source = UI.ReadChessPosition().ToPosition();
                    Console.Write("Target: ");
                    Position target = UI.ReadChessPosition().ToPosition();

                    chessMatch.PeformMove(source, target);
                }

            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
