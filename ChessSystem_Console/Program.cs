using Boardgame;
using Chess;
using System;

namespace ChessSystem_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Board board = new Board(8, 8);

                board.PlacePiece(new Tower(board, Color.Black), new Position(0, 0));
                board.PlacePiece(new Tower(board, Color.Black), new Position(1, 3));
                board.PlacePiece(new King(board, Color.Black), new Position(0, 2));

                board.PlacePiece(new Tower(board, Color.White), new Position(6, 3));
                board.PlacePiece(new Tower(board, Color.White), new Position(7, 4));
                board.PlacePiece(new King(board, Color.White), new Position(6, 0));

                UI.PrintBoard(board);
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
