using System;
using Microsoft.AspNetCore.SignalR;
using Snake;

namespace SignalRChat.Hubs
{
	public class DirectionHub : Hub
	{


        // Fields
        private readonly ILogger<DirectionHub> _logger;


        // Constructor
        public DirectionHub(ILogger<DirectionHub> logger)
        {
            _logger = logger;
        }


        public void ClientDirection(string key)
        {
            if (GameLoop.gameState.GameOver)
            {
                return;
            }

            switch (key)
            {
                case "ArrowUp":
                    GameLoop.gameState.ChangeDirection(Direction.Up);
                    break;

                case "ArrowDown":
                    GameLoop.gameState.ChangeDirection(Direction.Down);
                    break;

                case "ArrowLeft":
                    GameLoop.gameState.ChangeDirection(Direction.Left);
                    break;

                case "ArrowRight":
                    GameLoop.gameState.ChangeDirection(Direction.Right);
                    break;
            }
        }
    }
}