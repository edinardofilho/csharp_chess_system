using Boardgame;
using System.Collections.Generic;

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
            Piece p = Board.RemovePiece(source);
            p.IncreaseMoveCount();
            Piece capturedPiece = Board.RemovePiece(target);
            Board.PlacePiece(p, target);
            if (capturedPiece != null)
            {
                Captured.Add(capturedPiece);
            }

            // #Special move - Castling - Kingside Rook

            if (p is King && target.Column == source.Column + 2)
            {
                Position rookSource = new Position(source.Row, source.Column + 3);
                Position rookTarget = new Position(source.Row, source.Column + 1);
                Piece rook = Board.RemovePiece(rookSource);
                rook.IncreaseMoveCount();

                Board.PlacePiece(rook, rookTarget);
            }

            // #Special move - Castling - Queenside Rook

            if (p is King && target.Column == source.Column - 2)
            {
                Position rookSource = new Position(source.Row, source.Column - 4);
                Position rookTarget = new Position(source.Row, source.Column - 1);
                Piece rook = Board.RemovePiece(rookSource);
                rook.IncreaseMoveCount();

                Board.PlacePiece(rook, rookTarget);
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

            // #Special move - Castling - Kingside Rook

            if (p is King && target.Column == source.Column + 2)
            {
                Position rookSource = new Position(source.Row, source.Column + 3);
                Position rookTarget = new Position(source.Row, source.Column + 1);
                Piece rook = Board.RemovePiece(rookTarget);
                rook.DecreaseMoveCount();

                Board.PlacePiece(rook, rookSource);
            }

            // #Special move - Castling - Queenside Rook

            if (p is King && target.Column == source.Column - 2)
            {
                Position rookSource = new Position(source.Row, source.Column - 4);
                Position rookTarget = new Position(source.Row, source.Column - 1);
                Piece rook = Board.RemovePiece(rookTarget);
                rook.DecreaseMoveCount();

                Board.PlacePiece(rook, rookSource);
            }
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

            if (IsACheckMate(OpponentPlayer(CurrentPlayer)))
            {
                GameOver = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }
        }

        public void CertifySource(Position position)
        {
            if (Board.Piece(position) == null)
            {
                throw new BoardException("There is no p on choosen source position!");
            }
            if (CurrentPlayer != Board.Piece(position).Color)
            {
                throw new BoardException("Source p is not yours!");
            }
            if (!Board.Piece(position).ThereIsPossibleMoves())
            {
                throw new BoardException("There are no possible moves for the choosen source p!");
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

        public bool IsACheckMate(Color color)
        {
            if (!IsACheck(color))
            {
                return false;
            }
            foreach (Piece p in AvailablePieces(color))
            {
                bool[,] mat = p.PossibleMoves();

                for (int i = 0; i < Board.Rows; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position source = p.Position;
                            Position target = new Position(i, j);
                            Piece capturedPiece = PerformMove(source, target);
                            bool check = IsACheck(color);
                            UndoMove(source, target, capturedPiece);
                            if (!check)
                            {
                                return false;
                            }

                        }
                    }
                }
            }
            return true;
        }

        public void PlaceNewPiece(char column, int row, Piece p)
        {
            Board.PlacePiece(p, new ChessPosition(column, row).ToPosition());
            Pieces.Add(p);
        }

        private void PlacePiece()
        {
            PlaceNewPiece('a', 1, new Rook(Board, Color.White));
            PlaceNewPiece('b', 1, new Knight(Board, Color.White));
            PlaceNewPiece('c', 1, new Bishop(Board, Color.White));
            PlaceNewPiece('d', 1, new Queen(Board, Color.White));
            PlaceNewPiece('e', 1, new King(Board, Color.White, this));
            PlaceNewPiece('f', 1, new Bishop(Board, Color.White));
            PlaceNewPiece('g', 1, new Knight(Board, Color.White));
            PlaceNewPiece('h', 1, new Rook(Board, Color.White));
            PlaceNewPiece('a', 2, new Pawn(Board, Color.White, this));
            PlaceNewPiece('b', 2, new Pawn(Board, Color.White, this));
            PlaceNewPiece('c', 2, new Pawn(Board, Color.White, this));
            PlaceNewPiece('d', 2, new Pawn(Board, Color.White, this));
            PlaceNewPiece('e', 2, new Pawn(Board, Color.White, this));
            PlaceNewPiece('f', 2, new Pawn(Board, Color.White, this));
            PlaceNewPiece('g', 2, new Pawn(Board, Color.White, this));
            PlaceNewPiece('h', 2, new Pawn(Board, Color.White, this));

            PlaceNewPiece('a', 8, new Rook(Board, Color.Black));
            PlaceNewPiece('b', 8, new Knight(Board, Color.Black));
            PlaceNewPiece('c', 8, new Bishop(Board, Color.Black));
            PlaceNewPiece('d', 8, new Queen(Board, Color.Black));
            PlaceNewPiece('e', 8, new King(Board, Color.Black, this));
            PlaceNewPiece('f', 8, new Bishop(Board, Color.Black));
            PlaceNewPiece('g', 8, new Knight(Board, Color.Black));
            PlaceNewPiece('h', 8, new Rook(Board, Color.Black));
            PlaceNewPiece('a', 7, new Pawn(Board, Color.Black, this));
            PlaceNewPiece('b', 7, new Pawn(Board, Color.Black, this));
            PlaceNewPiece('c', 7, new Pawn(Board, Color.Black, this));
            PlaceNewPiece('d', 7, new Pawn(Board, Color.Black, this));
            PlaceNewPiece('e', 7, new Pawn(Board, Color.Black, this));
            PlaceNewPiece('f', 7, new Pawn(Board, Color.Black, this));
            PlaceNewPiece('g', 7, new Pawn(Board, Color.Black, this));
            PlaceNewPiece('h', 7, new Pawn(Board, Color.Black, this));
        }
    }
}
