using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snake.Data;

namespace Snake.Pages;

public class IndexModel : PageModel
{
    // Dictionary 
    public readonly Dictionary<GridValue, string> gridValToImage = new()
    {
        { GridValue.Empty, "/assets/empty.svg" },
        { GridValue.Snake, "/assets/snake.svg" },
        { GridValue.Food, "/assets/food.svg" }
    };

    // Fields
    private static readonly int rows = 12, cols = 24;
    private readonly ILogger<IndexModel> _logger;
    public GameState gameState = new GameState(rows, cols);
    public GridValue gridVal = GridValue.Empty;


    // Properties
    public int Rows { get; }
    public int Cols { get; }



    // Constructor
    public IndexModel(ILogger<IndexModel> logger)
    {
        Rows = rows;
        Cols = cols;
        _logger = logger;
    }


    // Returns a list of <img> tags with the correct image in the src as total strings 
    public List<string> DrawGrid()
    {
        List<string> imgTags = new();
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                gridVal = gameState.Grid[r, c];
                string imageUrl = gridValToImage[gridVal];
                imgTags.Add($"<img src='{imageUrl}' alt=\"Grid Image\">");
            }
        }
        return imgTags;
    }


    // Handle arrow key input AJAX calls
    public void OnPostHandleArrowKeyPress(string arrowDirection)
    {
        switch (arrowDirection)
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


    public async Task GameLoop()
    {
        // While gameState.GameOver is true call gameState.Move(); every 100ms
        while (!gameState.GameOver)
        {
            await Task.Delay(100);
            // Call the fucntion that loops through all cols and rows and 
            gameState.Move();
            DrawGrid();
        }
    }
}