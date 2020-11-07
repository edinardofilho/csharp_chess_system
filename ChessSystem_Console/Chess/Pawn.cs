using Boardgame;

namespace Chess
{
    class Pawn : Piece
    {
        public ChessMatch ChessMatch { get; private set; }

        public Pawn(Board board, Color color, ChessMatch chessMatch) : base(board, color)
        {
            ChessMatch = chessMatch;
        }

        private bool ThereIsAnEnemy(Position position)
        {
            Piece piece = Board.Piece(position);
            return piece != null && piece.Color != Color;
        }

        private bool EmptyPosition(Position position)
        {
            return Board.Piece(position) == null;
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] mat = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);

            if (Color == Color.White)
            {
                position.SetValues(Position.Row - 1, Position.Column);
                if (Board.ValidPosition(position) && EmptyPosition(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.SetValues(Position.Row - 2, Position.Column);
                if (Board.ValidPosition(position) && EmptyPosition(position) && MoveCount == 0)
                {
                    mat[position.Row, position.Column] = true;
                }

                position.SetValues(Position.Row - 1, Position.Column - 1);
                if (Board.ValidPosition(position) && ThereIsAnEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.SetValues(Position.Row - 1, Position.Column + 1);
                if (Board.ValidPosition(position) && ThereIsAnEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                // #Special move - En Passant

                if(Position.Row == 3)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.ValidPosition(left) && ThereIsAnEnemy(left) && Board.Piece(left) == ChessMatch.EnPassantVulnerable)
                    {
                        mat[left.Row - 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.ValidPosition(right) && ThereIsAnEnemy(right) && Board.Piece(right) == ChessMatch.EnPassantVulnerable)
                    {
                        mat[right.Row - 1, right.Column] = true;
                    }
                }
            }
            else
            {
                position.SetValues(Position.Row + 1, Position.Column);
                if (Board.ValidPosition(position) && EmptyPosition(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.SetValues(Position.Row + 2, Position.Column);
                if (Board.ValidPosition(position) && EmptyPosition(position) && MoveCount == 0)
                {
                    mat[position.Row, position.Column] = true;
                }

                position.SetValues(Position.Row + 1, Position.Column - 1);
                if (Board.ValidPosition(position) && ThereIsAnEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.SetValues(Position.Row + 1, Position.Column + 1);
                if (Board.ValidPosition(position) && ThereIsAnEnemy(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                // #Special move - En Passant

                if (Position.Row == 4)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.ValidPosition(left) && ThereIsAnEnemy(left) && Board.Piece(left) == ChessMatch.EnPassantVulnerable)
                    {
                        mat[left.Row + 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.ValidPosition(right) && ThereIsAnEnemy(right) && Board.Piece(right) == ChessMatch.EnPassantVulnerable)
                    {
                        mat[right.Row + 1, right.Column] = true;
                    }
                }
            }

            return mat;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}
