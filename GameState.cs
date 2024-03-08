using SignalRChat.Hubs;
using Snake.Data;

namespace Snake
{
	public class GameState
	{
        public int Rows { get; }
        public int Cols { get; }
		public GridValue[,] Grid { get; }	// The game grid itself
		public Direction Dir { get; set; }
		public int Score { get; private set; }
		public bool GameOver { get; private set; }


		public readonly LinkedList<Direction> dirChnages = new();
        private readonly LinkedList<Position> snakePositions = new();
		private readonly Random random = new();


        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
			Dir = Direction.Right;	// Set starting direction of snake
			AddSnake();
			AddFood();
        }


        private void AddSnake()
		{
            int r = Rows / 2;
            for (int c = 1; c <= 3; c++)
            {
				Grid[r, c] = GridValue.Snake;
				snakePositions.AddFirst(new Position(r, c));
            }
        }


		private IEnumerable<Position> EmptyPositions()
		{
			for(int r = 0; r < Rows; r++)
			{
				for(int c = 0; c < Cols; c++)
				{
					if (Grid[r, c] == GridValue.Empty)
					{
						yield return new Position(r, c);
					}
				}
			}
		}


		private void AddFood()
		{
			List<Position> empty = new List<Position>(EmptyPositions()); 
			if (empty.Count == 0)	// Game has been beaten
            {
				return;
			}
			Position pos = empty[random.Next(empty.Count)];
			Grid[pos.Row, pos.Col] = GridValue.Food;
		}


		private Position HeadPosition()
		{
			return snakePositions.First.Value;
		}


		private Position TailPosition()
		{
			return snakePositions.Last.Value;
		}


		private void AddHead(Position pos)
		{
			snakePositions.AddFirst(pos);
			Grid[pos.Row, pos.Col] = GridValue.Snake;
		}


		private void RemoveTail()
		{
			Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
			snakePositions.RemoveLast();
        }


		private Direction GetLastDirection()
		{
			if (dirChnages.Count == 0)
			{
				return Dir;
			}
			return dirChnages.Last.Value;
		}


		private bool CanChnageDirection(Direction newDir)
		{
			if(dirChnages.Count == 2)
			{
				return false;
			}
			Direction lastDir = GetLastDirection();
			return newDir != lastDir && newDir != lastDir.Opposite();
		}


		public void ChangeDirection(Direction dir)
		{
			if (CanChnageDirection(dir))
			{
                dirChnages.AddLast(dir);
            }
		}


		// Check if position is outside the grid
		private bool OutsideGrid(Position pos)
		{
			return pos.Row < 0 || pos.Col < 0 || pos.Row >= GameHub.rows || pos.Col >= GameHub.cols;
        }


		private GridValue WillHit(Position newHeadPos)
		{
			if (OutsideGrid(newHeadPos))
			{
				return GridValue.Outside;
			}
			else if (newHeadPos == TailPosition())
			{
				return GridValue.Empty;
			}
			return Grid[newHeadPos.Row, newHeadPos.Col];
		}


		// Move snake one space in the current direction
		public void Move()
		{
			if (dirChnages.Count > 0)
			{
				Dir = dirChnages.First.Value;
				dirChnages.RemoveFirst();
			}

			Position newHeadPos = HeadPosition().Translate(Dir);
			GridValue hit = WillHit(newHeadPos);

			if (hit == GridValue.Outside || hit == GridValue.Snake)
			{
				GameOver = true;
			}
			else if (hit == GridValue.Empty)
			{
				RemoveTail();
				AddHead(newHeadPos);
			}
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
				Score++;
				AddFood();
            }
        }
    }
}