using Boardgame;
using Chess;
using System;

namespace ChessSystem_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(8, 8);
            try
            {
                board.PlacePiece(new Tower(board, Color.Black), new Position(0, 0));
                board.PlacePiece(new Tower(board, Color.Black), new Position(1, 3));
                board.PlacePiece(new King(board, Color.Black), new Position(2, 4));
                board.PlacePiece(new Tower(board, Color.Black), new Position(2, 9));

                UI.PrintBoard(board);

                Console.ReadLine();
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }          
        }
    }
}
