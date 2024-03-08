using Microsoft.AspNetCore.SignalR;
using Snake;
using Snake.Data;

namespace SignalRChat.Hubs
{
    public class GameHub : Hub
    {
        private readonly Dictionary<GridValue, string> gridValToImage = new()
        {
            { GridValue.Empty, "/assets/empty.svg" },
            { GridValue.Snake, "/assets/snake.svg" },
            { GridValue.Food, "/assets/food.svg" }
        };


        public static readonly int rows = 12, cols = 20;
        public static GameState gameState = new (rows, cols);


        private List<string> DrawGrid()
        {
            List<string> imgTags = new() { };
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    string imageUrl = gridValToImage[gameState.Grid[r, c]];
                    imgTags.Add($"{imageUrl}");
                }
            }
            return imgTags;
        }


        public async Task StartLoop(int Level)
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(Level);
                gameState.Move();
                await Clients.All.SendAsync("UpdateUi", DrawGrid(), gameState.Score, gameState.GameOver);
            }
        }


        public void GameOver()
        {
            gameState = new GameState(rows, cols);
        }
    }
}