"use strict";

// Connect to SignalR Hub.
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Receive data from server
connection.on("UpdateUi", function (list, gameOver) {
    let i = 0
    for (let r = 0; r < 12; r++) {
        for (let c = 0; c < 20; c++) {
            document.getElementById(`img_${r}-${c}`).src = list[i];
            i++;
        }
    }
})

// Send data to server
function invokeGameLoop() {
    connection.invoke("GameLoop");
}


document.addEventListener("keydown", function (event) {
    if (event.key === "ArrowUp") {
        clientDirection("Up");
    } else if (event.key === "ArrowDown") {
        clientDirection("Down");
    } else if (event.key === "ArrowLeft") {
        clientDirection("Left");
    } else if (event.key === "ArrowRight") {
        clientDirection("Right");
    }
});


function clientDirection(dir) {
    connection.invoke("ClientDirection", dir)
}


connection.start()
    .then(() => {
        console.log("Connection established.");
        invokeGameLoop();
    })
    .catch((err) => {
        return console.error(err.toString());
    });