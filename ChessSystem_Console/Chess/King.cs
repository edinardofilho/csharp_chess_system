using Boardgame;

namespace Chess
{
    class King : Piece
    {
        public ChessMatch ChessMatch { get; private set; }

        public King(Board board, Color color, ChessMatch chessMatch) : base(board, color)
        {
            ChessMatch = chessMatch;
        }

        private bool CanMove(Position position)
        {
            Piece piece = Board.Piece(position);
            return piece == null || piece.Color != Color;
        }

        private bool TestRookCastling(Position position)
        {
            Piece piece = Board.Piece(position);
            return piece != null && piece is Rook && piece.Color == Color && piece.MoveCount == 0;
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] mat = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);

            // Above
            position.SetValues(Position.Row - 1, Position.Column);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // NE
            position.SetValues(Position.Row - 1, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // Right
            position.SetValues(Position.Row, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // SE
            position.SetValues(Position.Row + 1, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // Below
            position.SetValues(Position.Row + 1, Position.Column);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // SO
            position.SetValues(Position.Row + 1, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // Left
            position.SetValues(Position.Row, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // NO
            position.SetValues(Position.Row - 1, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Row, position.Column] = true;
            }

            // #Special move - Castling
            if (MoveCount == 0 && !ChessMatch.Check)
            {
                // #Special move - Castling - Kingside Rook
                Position positionR1 = new Position(Position.Row, Position.Column + 3);
                if (TestRookCastling(positionR1))
                {
                    Position position1 = new Position(Position.Row, Position.Column + 1);
                    Position position2 = new Position(Position.Row, Position.Column + 2);

                    if (Board.Piece(position1) == null && Board.Piece(position2) == null)
                    {
                        mat[Position.Row, Position.Column + 2] = true;
                    }
                }

                // #Special move - Castling - Queenside Rook
                Position positionR2 = new Position(Position.Row, Position.Column - 4);
                if (TestRookCastling(positionR2))
                {
                    Position position1 = new Position(Position.Row, Position.Column - 1);
                    Position position2 = new Position(Position.Row, Position.Column - 2);
                    Position position3 = new Position(Position.Row, Position.Column - 3);

                    if (Board.Piece(position1) == null && Board.Piece(position2) == null && Board.Piece(position3) == null)
                    {
                        mat[Position.Row, Position.Column - 2] = true;
                    }
                }
            }

            return mat;
        }

        public override string ToString()
        {
            return "K";
        }
    }
}
