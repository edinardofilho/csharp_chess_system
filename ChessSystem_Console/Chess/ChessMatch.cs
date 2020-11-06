using Boardgame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    class ChessMatch
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool GameOver { get; set; }
        public List<Piece> Pieces { get; set; }
        public List<Piece> Captured { get; set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            GameOver = false;
            Pieces = new List<Piece>();
            Captured = new List<Piece>();
            PlacePiece();
        }

        public void PerformMove(Position source, Position target)
        {
            Piece piece = Board.RemovePiece(source);
            piece.IncreaseMoveCount();
            Piece capturedPiece = Board.RemovePiece(target);
            Board.PlacePiece(piece, target);
            if (capturedPiece != null)
            {
                Captured.Add(capturedPiece);
            }
        }

        public void PerformChessMove(Position source, Position target)
        {
            PerformMove(source, target);
            Turn++;
            ChangePlayer();
        }

        public void CertifySource(Position position)
        {
            if (Board.Piece(position) == null)
            {
                throw new BoardException("There is no piece on choosen source position!");
            }
            if (CurrentPlayer != Board.Piece(position).Color)
            {
                throw new BoardException("Source piece is not yours!");
            }
            if (!Board.Piece(position).ThereIsPossibleMoves())
            {
                throw new BoardException("There are no possible moves for the choosen source piece!");
            }
        }

        public void CertifyTarget(Position source, Position target)
        {
            if (!Board.Piece(source).AllowedPosition(target))
            {
                throw new BoardException("Invalid target position!");
            }
        }

        private void ChangePlayer()
        {
            if (CurrentPlayer == Color.White)
            {
                CurrentPlayer = Color.Black;
            }
            else
            {
                CurrentPlayer = Color.White;
            }
        }

        public List<Piece> CapturedPieces(Color color)
        {
            List<Piece> aux = new List<Piece>();
            foreach(Piece p in Captured)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }

            return aux;
        }

        public List<Piece> AvailablePieces(Color color)
        {
            List<Piece> aux = new List<Piece>();
            foreach (Piece p in Pieces)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }

            aux.Except(CapturedPieces(color));
            return aux;
        }

        public void PlaceNewPiece(char column, int row, Piece piece)
        {
            Board.PlacePiece(piece, new ChessPosition(column, row).ToPosition());
            Pieces.Add(piece);
        }

        private void PlacePiece()
        {
            PlaceNewPiece('c', 1, new Rook(Board, Color.White));
            PlaceNewPiece('c', 2, new Rook(Board, Color.White));
            PlaceNewPiece('d', 2, new Rook(Board, Color.White));
            PlaceNewPiece('e', 2, new Rook(Board, Color.White));
            PlaceNewPiece('e', 1, new Rook(Board, Color.White));
            PlaceNewPiece('d', 1, new King(Board, Color.White));

            PlaceNewPiece('c', 7, new Rook(Board, Color.Black));
            PlaceNewPiece('c', 8, new Rook(Board, Color.Black));
            PlaceNewPiece('d', 7, new Rook(Board, Color.Black));
            PlaceNewPiece('e', 7, new Rook(Board, Color.Black));
            PlaceNewPiece('e', 8, new Rook(Board, Color.Black));
            PlaceNewPiece('d', 8, new King(Board, Color.Black));
        }
    }
}
