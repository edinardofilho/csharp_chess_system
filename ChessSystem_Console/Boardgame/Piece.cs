namespace Boardgame
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MoveCount { get; protected set; }
        public Board Board { get; protected set; }

        public Piece(Board board, Color color)
        {
            Position = null;
            Color = color;
            Board = board;
            MoveCount = 0;
        }

        public bool ThereIsPossibleMoves()
        {
            bool[,] mat = PossibleMoves();
            for (int i = 0; i < Board.Rows; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AllowedPosition(Position position)
        {
            return PossibleMoves()[position.Row, position.Column];
        }

        public abstract bool[,] PossibleMoves();

        public void IncreaseMoveCount()
        {
            MoveCount++;
        }

        public void DecreaseMoveCount()
        {
            MoveCount--;
        }
    }
}
