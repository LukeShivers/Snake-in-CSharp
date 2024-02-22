using System;

namespace Snake
{
	public class Direction
	{


        // Fields (variables) shared among all classes
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);


		// Properties
        public int RowOffset { get; }
		public int ColOffset { get; }


		// Constructor
		private Direction(int rowOffset, int colOffset)
		{
			RowOffset = rowOffset;
			ColOffset = colOffset;
		}


		// Constructor for Opposite Direction
		public Direction Opposite()
		{
			return new Direction(-RowOffset, -ColOffset);
		}


        // Called the "Equal Method" - Determines whether two object instances are equal.
        public override bool Equals(object? obj)
        {
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;
        }


        // Overrides default GetHashCode() So the direction class can be used in a dictionary.
        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffset, ColOffset);
        }


        // Overload of equality operator. Used to compare directions
        public static bool operator ==(Direction? left, Direction? right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }


        // Overload of inequality operator. Used to compare directions.
        public static bool operator !=(Direction? left, Direction? right)
        {
            return !(left == right);
        }
    }
}