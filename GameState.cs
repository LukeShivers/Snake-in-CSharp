﻿using System;
using Snake.Data;

namespace Snake
{
	public class GameState
	{
        // Properties
        public int Rows { get; }
        public int Cols { get; }
		public GridValue[,] Grid { get; } // The Grid itself, a 2-D array of GridValues.
		public Direction Dir { get; private set; } // Can be access from anywhere but only set here
		public int Score { get; private set; }
		public bool GameOver { get; private set; }


		// Initalized list of Positions (x's,y's) currently occupied by the snake.
		private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();


		// Food spawn
		private readonly Random random = new Random();


		// Constructor
        public GameState(int rows, int cols)
        {
            Rows = rows; // Updates Rows variable to the number of rows given
            Cols = cols; // Updates Cols variable to the number of cols given
            Grid = new GridValue[rows, cols]; // Calls the 2-D array grid with certain# of rows and certain# of cols. 
			Dir = Direction.Right; // Start snake right

			AddSnake();
			AddFood();
        }


		// A method that sets starting rows&cols for snake and adds snake to the grid & the list
		private void AddSnake()
		{
            // Adds it to the middle row
            int r = Rows / 2;

			// Loops over colummns 1,2,3
            for (int c = 1; c <= 3; c++)
            {
				Grid[r, c] = GridValue.Snake; // Updates Grid array to add snake square to row# row/2 and col# 1,2,3.
				snakePositions.AddFirst(new Position(r, c)); // Add snake squares to the start of the list after each iteration of loop
            }
        }


		// Create a method that iterates though all grid positions and returns Position objects for each empty grid position.
		private IEnumerable<Position> EmptyPositions()
		{
			for(int r = 0; r < Rows; r++)
			{
				for(int c = 0; c < Cols; c++)
				{
					if (Grid[r, c] == GridValue.Empty) // If the value at grid value r, c (loop) is empty then...
					{
						yield return new Position(r, c);
					}
				}
			}
		}


		// Adds food square 
		private void AddFood()
		{
			// List called 'empty' of empty grid squares 
			List<Position> empty = new List<Position>(EmptyPositions()); 

			// If no empty squares (game beat)
			if (empty.Count == 0)
			{
				return;
			}

			// returns a Position obj from the 'empty' list between 0 and the number of empty squares.
			Position pos = empty[random.Next(empty.Count)];
			// sets GridValue.Food = to that row and col.
			Grid[pos.Row, pos.Col] = GridValue.Food;
		}


		// Get positon of snake head
		public Position HeadPosition()
		{
			// Get the last element in the linked list
			return snakePositions.First.Value;
		}


		public Position TailPosition()
		{
			return snakePositions.Last.Value;
		}


		// Returns all the position values of the snake 
		public IEnumerable<Position> SnakePosition()
		{
			return snakePositions;
		}

		// Adds head to new sqaure when snake moves
		public void AddHead(Position pos)
		{
			// Add a value to the beginning of the list
			snakePositions.AddFirst(pos);
			// Add snake to the grid value 
			Grid[pos.Row, pos.Col] = GridValue.Snake;
		}

		// Removes the tail from the previous grid square
		public void RemoveTail()
		{
			Position tail = snakePositions.Last.Value;
			// Set the grid value to empty
            Grid[tail.Row, tail.Col] = GridValue.Empty;
			snakePositions.RemoveLast();
        }


		public void ChnageDirections(Direction dir)
		{
			Dir = dir;
		}

		// Checks if parameter is outside the grid
		private bool OutsideGrid(Position pos)
		{
			return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Rows;
        }

		// Returns what the snake will hit when it moves
		private GridValue WillHit(Position newHeadPos)
		{
			// If snake hits outside parameter
			if (OutsideGrid(newHeadPos))
			{
				return GridValue.Outside;
			}

			// If head almost hits tail
			if (newHeadPos == TailPosition())
			{
				return GridValue.Empty;
			}

			// If snake doesn't hit anything return that position.
			return Grid[newHeadPos.Row, newHeadPos.Col];
		}

		// Move snake one space in the current direction
		public void move()
		{
			Position newHeadPos = HeadPosition().Translate(Dir);
			GridValue hit = WillHit(newHeadPos);	// Set hit = to the hit function plugging in the direction

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

