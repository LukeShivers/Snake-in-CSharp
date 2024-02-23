using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snake.Data;

namespace Snake.Pages;

public class IndexModel : PageModel
{
    // A simple dictionary that correlates binary values to images



    //public readonly Dictionary<GridValue, string> gridValToImage = new()
    //{
    //    { GridValue.Empty, "~/assets/empty.svg" },
    //    { GridValue.Snake, "~/assets/snake.svg" },
    //    { GridValue.Food, "~/assets/food.svg" }
    //};


    //public readonly int rows = 12, cols = 24;
    //public GameState gameState;    // creates variable


    //public IndexModel()
    //{
    //    gameState = new GameState(rows, cols);  // initalizes new Grid array with snake & food binarys and by default empty binarys
    //}



    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}

