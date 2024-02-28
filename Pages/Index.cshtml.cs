using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snake.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }


    // Updates the GameState obj based on arrow keystrokes
    //public void OnPostHandleArrowKeyPress(string arrowDirection)
    //{
    //    switch (arrowDirection)
    //    {
    //        case "Up":
    //            gameState.ChangeDirection(Direction.Up);
    //            break;
    //        case "Down":
    //            gameState.ChangeDirection(Direction.Down);
    //            break;
    //        case "Left":
    //            gameState.ChangeDirection(Direction.Left);
    //            break;
    //        case "Right":
    //            gameState.ChangeDirection(Direction.Right);
    //            break;
    //    }
    //}
}