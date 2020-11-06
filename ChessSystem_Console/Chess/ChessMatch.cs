using Boardgame;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    class ChessMatch
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }
        public bool Check { get; private set; }
        public HashSet<Piece> Pieces { get; private set; }
        public HashSet<Piece> Captured { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            GameOver = false;
            Check = false;
            Pieces = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            PlacePiece();
        }

        public Piece PerformMove(Position source, Position target)
        {
            Piece piece = Board.RemovePiece(source);
            piece.IncreaseMoveCount();
            Piece capturedPiece = Board.RemovePiece(target);
            Board.PlacePiece(piece, target);
            if (capturedPiece != null)
            {
                Captured.Add(capturedPiece);
            }

            return capturedPiece;
        }

        private void UndoMove(Position source, Position target, Piece capturedPiece)
        {
            Piece p = Board.RemovePiece(target);
            p.DecreaseMoveCount();
            if (capturedPiece != null)
            {
                Board.PlacePiece(capturedPiece, target);
                Captured.Remove(capturedPiece);
            }
            Board.PlacePiece(p, source);
        }

        public void PerformChessMove(Position source, Position target)
        {
            Piece capturedPiece = PerformMove(source, target);
            if (IsACheck(CurrentPlayer))
            {
                UndoMove(source, target, capturedPiece);
                throw new BoardException("You can't put yourself in Check!");
            }

            if (IsACheck(OpponentPlayer(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
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

        public HashSet<Piece> CapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in Captured)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }

            return aux;
        }

        public HashSet<Piece> AvailablePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in Pieces)
            {
                if (p.Color == color)
                {
                    aux.Add(p);
                }
            }

            aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        private Color OpponentPlayer(Color color)
        {
            if (color == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private Piece IsAKing(Color color)
        {
            foreach (Piece p in AvailablePieces(color))
            {
                if (p is King)
                {
                    return p;
                }
            }
            return null;
        }

        public bool IsACheck(Color color)
        {
            Piece k = IsAKing(color);
            if (k == null)
            {
                throw new BoardException("There is no" + color + "king on board!");
            }

            foreach (Piece p in AvailablePieces(OpponentPlayer(color)))
            {
                bool[,] mat = p.PossibleMoves();
                if (mat[k.Position.Row, k.Position.Column])
                {
                    return true;
                }
            }
            return false;
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
