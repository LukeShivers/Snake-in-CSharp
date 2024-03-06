// DOM Selectors
const volume = document.getElementById("volume");
const overlay = document.querySelector(".overlay");
const loadContainer = document.getElementById("loadContainer");
const easyBtn = document.getElementById("easyBtn");
const mediumBtn = document.getElementById("mediumBtn");
const hardBtn = document.getElementById("hardBtn");
const btn = document.querySelectorAll(".btn");
const countdownTxt = document.getElementById("countdownTxt");
const uiScore = document.querySelector(".score");
const uiLevel = document.querySelector(".level");
const gameOverContainer = document.getElementById("gameOverContainer");
const changeLevelBtn = document.getElementById("changeLevelBtn");


// Variables & flags
const rows = 12;
const cols = 20;
let dead = false;
let currentLevel;
let previousScore = 0;
let noiseOn = true;


// SignalR Config
var gameLoopConnection = new signalR.HubConnectionBuilder().withUrl("/gameLoopHub").build();
var directionConnection = new signalR.HubConnectionBuilder().withUrl("/directionHub").build();


// Event Listeners
volume.addEventListener("click", () => {
    playAudio("/assets/volumeClick.mp3");
    if (volume.classList.contains("noiseOn")) {
        noiseOn = false;
        volume.src = '/assets/noVolume.svg';
        volume.classList.remove("noiseOn")
    } else {
        noiseOn = true;
        volume.src = '/assets/volume.svg';
        volume.classList.add("noiseOn");
    }
})
easyBtn.addEventListener("click", () => {
    if (noiseOn) {
        playAudio("/assets/buttonClick.mp3");
    }
    initSequence("Easy", 250)
});
mediumBtn.addEventListener("click", () => {
    if (noiseOn) {
        playAudio("/assets/buttonClick.mp3");
    }
    initSequence("Medium", 175)
})
hardBtn.addEventListener("click", () => {
    if (noiseOn) {
        playAudio("/assets/buttonClick.mp3");
    }
    initSequence("Hard", 100)
})
changeLevelBtn.addEventListener("click", () => {
    if (noiseOn) {
        playAudio("/assets/buttonClick.mp3");
    }
    gameOverContainer.style.display = "none";
    loadContainer.style.display = "flex";
})


// Hides inital home screen
function initSequence(difficulty, level) {
    setInitBoard();
    loadContainer.style.display = "none";
    uiLevel.textContent = `Level: ${difficulty}`;
    countdown(level);
}


// Creates a 1s delay
function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}


// Initiates countdown and sends server req
async function countdown(level) {
    currentLevel = level;
    dead = false;
    countdownTxt.style.display = "flex";
    uiScore.textContent = "SCORE: 0"
    let i = 3;
    while (i >= 1) {
        if (noiseOn) {
            playAudio("/assets/volumeClick.mp3");
        }
        countdownTxt.textContent = i;
        await delay(1000);
        i--;
    }
    overlay.style.display = "none";
    countdownTxt.style.display = "none";
    gameLoopConnection.invoke("StartLoop", level);
}


// Sets the UI to a grid of empty squares
function setInitBoard() {
    let i = 0
    for (let r = 0; r < rows; r++) {
        for (let c = 0; c < cols; c++) {
            document.getElementById(`img_${r}-${c}`).src = "/assets/empty.svg";
            i++;
        }
    }
}


// Receives server res
gameLoopConnection.on("UpdateUi", (list, serverScore, gameOver) => {
    if (serverScore > previousScore && noiseOn) {
        playAudio('/assets/bite.mp3');
    }
    StartLoop(list);
    uiScore.textContent = `SCORE: ${serverScore * 5}`;
    if (gameOver) {
        gameLoopConnection.invoke("GameOver");
        GameOver();
    }
    previousScore = serverScore;
});


// Client-side game loop
function StartLoop(imgs) {
    let i = 0
    for (let r = 0; r < rows; r++) {
        for (let c = 0; c < cols; c++) {
            document.getElementById(`img_${r}-${c}`).src = imgs[i];
            i++;
        }
    }
}


// Volume clicks
function playAudio(link) {
    var audio = new Audio(link);
    audio.play();
}



// Read keystrokes
document.addEventListener("keydown", (e) => {
    if (dead) {
        gameOverContainer.style.display = "none"
        countdown(currentLevel);
    } else {
        directionConnection.invoke("ClientDirection", e.key)
    }
    e.preventDefault();
});


// Game over
async function GameOver() {
    if (noiseOn) {
        playAudio("/assets/fail.mp3");
    }
    await delay(1000);
    dead = true;
    overlay.style.display = "flex";
    gameOverContainer.style.display = "flex"
    setInitBoard();
}


// Check GameLoopHub connection
gameLoopConnection.start()
    .then(() => {
        console.log("GameLoopHub Connection Successful");
    })
    .catch((err) => {
        return console.error(err.toString());
    });


// Check DirectionHub connection
directionConnection.start()
    .then(() => {
        console.log("DirectionHub Connection Successful");
    })
    .catch((err) => {
        return console.error(err.toString());
    });