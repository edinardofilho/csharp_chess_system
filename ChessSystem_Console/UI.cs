using Boardgame;
using System;

namespace ChessSystem_Console
{
    public class UI
    {
        public static void PrintBoard(Board board)
        {
            for (int i = 0; i < board.Rows; i++)
            {
                Console.Write(8 - i + " ");

                for (int j = 0; j < board.Columns; j++)
                {
                    
                    if (board.Piece(i, j) == null)
                    {
                        Console.Write("- ");
                    }
                    else
                    {
                        UI.PrintPiece(board.Piece(i, j));
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece.Color == Color.White)
            {
                Console.Write(piece);
            } else
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(piece);
                Console.ForegroundColor = aux;
            }
        }
    }
}
