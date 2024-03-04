"use strict";

// Connect to SignalR Hub.
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


// Receive list of imgs from server
connection.on("UpdateUi", function (list) {
    let i = 0
    for (let r = 0; r < 12; r++) {
        for (let c = 0; c < 20; c++) {
            document.getElementById(`img_${r}-${c}`).src = list[i];
            i++;
        }
    }
});


// Send keystroke data to server.
document.addEventListener("keydown", (e) => {
    connection.invoke("ClientDirection", e.key)
});


async function gameLoop() {
    for (let i = 0; i < 8; i++) {
        await new Promise(resolve => setTimeout(resolve, 1000));
        console.log("Client going off")
        // Creates a whole new gameboard every second
        connection.invoke("GameLoop");
    }
} 


connection.start()
    .then(() => {
        console.log("Connection Successful");
        gameLoop();
    })
    .catch((err) => {
        return console.error(err.toString());
    });