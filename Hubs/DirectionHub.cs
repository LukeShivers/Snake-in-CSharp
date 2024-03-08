using Microsoft.AspNetCore.SignalR;
using Snake;

namespace SignalRChat.Hubs
{
	public class DirectionHub : Hub
	{
        public void ClientDirection(string key)
        {
            if (GameHub.gameState.GameOver) // For Game bring beat, not snake dying
            {
                return;
            }
            switch (key)
            {
                case "ArrowUp":
                    GameHub.gameState.ChangeDirection(Direction.Up);
                    break;
                case "ArrowDown":
                    GameHub.gameState.ChangeDirection(Direction.Down);
                    break;
                case "ArrowLeft":
                    GameHub.gameState.ChangeDirection(Direction.Left);
                    break;
                case "ArrowRight":
                    GameHub.gameState.ChangeDirection(Direction.Right);
                    break;
            }
        }
    }
}