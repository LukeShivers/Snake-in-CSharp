using System.Text;
using Microsoft.AspNetCore.SignalR;
using Snake;
using Snake.Data;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {

        // Dictionary 
        public readonly Dictionary<GridValue, string> gridValToImage = new()
        {
            { GridValue.Empty, "/assets/empty.svg" },
            { GridValue.Snake, "/assets/snake.svg" },
            { GridValue.Food, "/assets/food.svg" }
        };


        // Fields
        private static readonly int rows = 12, cols = 20;
        public GameState gameState = new GameState(rows, cols);
        public GridValue gridVal = GridValue.Empty;


        // Method sets up initial board by returning a List<> of completed img tags.
        public List<string> DrawGrid()
        {
            List<string> imgTags = new() { };
            for(int r = 0; r < 12; r++)
            {
                for (int c = 0; c < 20; c++)
                {
                    gridVal = gameState.Grid[r, c];     // Gets the binary value of an x,y in code version of game
                    string imageUrl = gridValToImage[gridVal];      // Sets a string var equal to the value in the dict
                    imgTags.Add($"{imageUrl}");      // adds an interpolation of that string value into an <img> template to List<>
                }
            }
            return imgTags;
        }


        // Method updates Client with new board 
        public async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(250);
                gameState.Move();
                await Clients.All.SendAsync("UpdateUi", DrawGrid(), !gameState.GameOver);   // Update client by drawing new grid after snake moved one position
            }
        }


        // Method updates direction
        public void ClientDirection(string dir)
        {
            if (gameState.GameOver)
            {
                return;
            }

            // when I get the direction call the corresponding
            switch (dir)
            {
                case "Up":
                    gameState.ChangeDirection(Direction.Up);
                    break;

                case "Down":
                    gameState.ChangeDirection(Direction.Down);
                    break;

                case "Left":
                    gameState.ChangeDirection(Direction.Left);
                    break;

                case "Right":
                    gameState.ChangeDirection(Direction.Right);
                    break;
            }

        }
    }
}

