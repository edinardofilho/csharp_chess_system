using Boardgame;

namespace Chess
{
    class Pawn : Piece
    {
        public Pawn(Board board, Color color) : base(board, color)
        {
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

                position.SetValues(Position.Row - 2, Position.Column + 1);
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
            }
            else
            {
                position.SetValues(Position.Row + 1, Position.Column);
                if (Board.ValidPosition(position) && EmptyPosition(position))
                {
                    mat[position.Row, position.Column] = true;
                }

                position.SetValues(Position.Row + 2, Position.Column + 1);
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
            }

            return mat;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}
