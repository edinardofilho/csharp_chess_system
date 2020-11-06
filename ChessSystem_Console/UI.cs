using Boardgame;
using Chess;
using System;
using System.Collections.Generic;

namespace ChessSystem_Console
{
    class UI
    {
        public static void PrintMatch(ChessMatch chessMatch)
        {
            PrintBoard(chessMatch.Board);
            Console.WriteLine();
            PrintCapturedPieces(chessMatch);
            Console.WriteLine();
            Console.WriteLine("Turn: " + chessMatch.Turn);
            if (!chessMatch.GameOver)
            {
                Console.WriteLine("Next player: " + chessMatch.CurrentPlayer);
                if (chessMatch.Check)
                {
                    Console.WriteLine("CHECK!");
                }
            } else
            {
                Console.WriteLine("CHECKMATE!");
                Console.WriteLine("Winner: " + chessMatch.CurrentPlayer);
            }
        }

        public static void PrintCapturedPieces(ChessMatch chessMatch)
        {
            Console.WriteLine("Captured pieces: ");
            Console.Write("White: ");
            PrintList(chessMatch.CapturedPieces(Color.White));
            Console.WriteLine();
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Black: ");
            PrintList(chessMatch.CapturedPieces(Color.Black));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void PrintList(HashSet<Piece> list)
        {
            Console.Write("[");
            foreach (Piece p in list)
            {
                Console.Write(p + " ");
            }
            Console.Write("]");
        }

        public static void PrintBoard(Board board)
        {
            for (int i = 0; i < board.Rows; i++)
            {
                Console.Write(8 - i + " ");

                for (int j = 0; j < board.Columns; j++)
                {
                    PrintPiece(board.Piece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void PrintBoard(Board board, bool[,] allowedPositions)
        {
            ConsoleColor originalBackgroundColor = Console.BackgroundColor;
            ConsoleColor newBackgroundColor = ConsoleColor.DarkGray;

            for (int i = 0; i < board.Rows; i++)
            {
                Console.Write(8 - i + " ");

                for (int j = 0; j < board.Columns; j++)
                {
                    if (allowedPositions[i, j])
                    {
                        Console.BackgroundColor = newBackgroundColor;
                    }
                    else
                    {
                        Console.BackgroundColor = originalBackgroundColor;
                    }
                    PrintPiece(board.Piece(i, j));
                    Console.BackgroundColor = originalBackgroundColor;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = originalBackgroundColor;
        }

        public static ChessPosition ReadChessPosition()
        {
            string s = Console.ReadLine();
            char column = s[0];
            int row = int.Parse(s[1].ToString());
            return new ChessPosition(column, row);
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (piece.Color == Color.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}
