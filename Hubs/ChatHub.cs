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
        private static readonly int rows = 12, cols = 20; // Central location to change # of rows & cols.
        public GameState gameState = new GameState(rows, cols);
        public GridValue gridVal = GridValue.Empty;
        private readonly ILogger<ChatHub> _logger;
        private Direction nextDirection = Direction.Right;



        // Constructor
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        

        // Method sets up initial board by returning a List<> of completed img tags.
        private List<string> DrawGrid()
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
            if (!gameState.GameOver)
            {
                gameState.ChangeDirection(nextDirection);   // Check for updates from ClientDirection() using events
                gameState.Move();
                await Clients.All.SendAsync("UpdateUi", DrawGrid());
            }
        }


        // Method updates the GameState object with new direction
        public void ClientDirection(string key)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (key)
            {
                case "ArrowUp":
                    nextDirection = Direction.Up;
                    break;

                case "ArrowDown":
                    nextDirection = Direction.Down;
                    break;

                case "ArrowLeft":
                    nextDirection = Direction.Left;
                    break;

                case "ArrowRight":
                    nextDirection = Direction.Right;
                    break;
            }
        }
    }
}