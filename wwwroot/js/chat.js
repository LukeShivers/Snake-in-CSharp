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
    console.log("Key: " + e.key)
    connection.invoke("ClientDirection", e.key)
});


connection.start()
    .then(() => {
        console.log("Connection Successful");
        connection.invoke("GameLoop");  // Send signal to server to start GameLoop
    })
    .catch((err) => {
        return console.error(err.toString());
    });