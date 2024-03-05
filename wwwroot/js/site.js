"use strict";

// Connect to SignalR Hubs.
var gameLoopConnection = new signalR.HubConnectionBuilder().withUrl("/gameLoopHub").build();
var directionConnection = new signalR.HubConnectionBuilder().withUrl("/directionHub").build();
let gameStarted = false;


// Receive list of imgs from server
gameLoopConnection.on("UpdateUi", function (list) {
    let i = 0
    for (let r = 0; r < 12; r++) {
        for (let c = 0; c < 20; c++) {
            document.getElementById(`img_${r}-${c}`).src = list[i];
            i++;
        }
    }
});


// Dynamically render score 
gameLoopConnection.on("ScoreUpdate", (serverScore) => {
    document.querySelector(".score").textContent = `SCORE: ${serverScore}`;
})


// Send keystroke data to server.
document.addEventListener("keydown", async (e) => {
    if (gameStarted) {
        let overlay = document.querySelector(".overlay");
        for (let i = 3; i > 0; i--) {
            await setTimeout(() => {
                console.log("iteration" + i)
                overlay.firstElementChild.textContent = i;
            }, 1000)
        };
        overlay.style.display = "none";
        gameLoopConnection.invoke("StartLoop");
    } else {
        directionConnection.invoke("ClientDirection", e.key)
        e.preventDefault();
    }
});


gameLoopConnection.start()
    .then(() => {
        console.log("GameLoop Connection Successful");
        gameStarted = true;
    })
    .catch((err) => {
        return console.error(err.toString());
    });


directionConnection.start()
    .then(() => {
        console.log("Direction Connection Successful");
    })
    .catch((err) => {
        return console.error(err.toString());
    });