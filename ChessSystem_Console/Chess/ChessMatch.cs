using Boardgame;
using System;

namespace Chess
{
    class ChessMatch
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool GameOver { get; set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            GameOver = false;
            PlacePiece();
        }

        private void PlacePiece()
        {
            Board.PlacePiece(new Rook(Board, Color.White), new ChessPosition('c', 1).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.White), new ChessPosition('c', 2).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.White), new ChessPosition('d', 2).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.White), new ChessPosition('e', 2).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.White), new ChessPosition('e', 1).ToPosition());
            Board.PlacePiece(new King(Board, Color.White), new ChessPosition('d', 1).ToPosition());

            Board.PlacePiece(new Rook(Board, Color.Black), new ChessPosition('c', 7).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.Black), new ChessPosition('c', 8).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.Black), new ChessPosition('d', 7).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.Black), new ChessPosition('e', 7).ToPosition());
            Board.PlacePiece(new Rook(Board, Color.Black), new ChessPosition('e', 8).ToPosition());
            Board.PlacePiece(new King(Board, Color.Black), new ChessPosition('d', 8).ToPosition());
        }

        public void PeformMove(Position source, Position target)
        {
            Piece piece = Board.RemovePiece(source);
            piece.IncreaseMoveCount();
            Piece capturedPiece = Board.RemovePiece(target);
            Board.PlacePiece(piece, target);
        }
    }
}
