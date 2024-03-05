using System.Text;
using Microsoft.AspNetCore.SignalR;
using Snake;
using Snake.Data;

namespace SignalRChat.Hubs
{
    public class GameLoop : Hub
    {


        // Dictionary 
        public readonly Dictionary<GridValue, string> gridValToImage = new()
        {
            { GridValue.Empty, "/assets/empty.svg" },
            { GridValue.Snake, "/assets/snake.svg" },
            { GridValue.Food, "/assets/food.svg" }
        };


        // Fields
        public static readonly int rows = 12, cols = 20; // Central location to change # of rows & cols.
        public static GameState gameState = new GameState(rows, cols);
        public GridValue gridVal = GridValue.Empty;
        private readonly ILogger<GameLoop> _logger;



        // Constructor
        public GameLoop(ILogger<GameLoop> logger)
        {
            _logger = logger;
        }


        // Runs game loop
        public async Task StartLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(150);
                gameState.Move();
                await Clients.All.SendAsync("UpdateUi", DrawGrid());
                await Clients.All.SendAsync("ScoreUpdate", gameState.Score);
            }
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
    }
}