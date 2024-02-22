using System;

namespace Snake
{
	public class Position
	{
        // Properties
        public int Row { get; }
        public int Col { get; }


		// Constructor takes a x,y 
        public Position(int row, int col)
		{
			Row = row;
			Col = col;
		}


		// Translate() takes a given direction, & returns the x,y for one position in that direction
		public Position Translate(Direction dir)
		{
			return new Position(Row + dir.RowOffset, Col + dir.ColOffset);
		}


        // See Direction.cs
        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Col == position.Col;
        }


        // See Direction.cs
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }


        // See Direction.cs
        public static bool operator ==(Position? left, Position? right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }


        // See Direction.cs
        public static bool operator !=(Position? left, Position? right)
        {
            return !(left == right);
        }
    }
}

