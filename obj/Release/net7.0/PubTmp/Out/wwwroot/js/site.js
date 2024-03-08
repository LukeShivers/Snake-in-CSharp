// DOM 
const volume = document.getElementById("volume");
const overlay = document.querySelector(".overlay");
const initLoadContainer = document.getElementById("initLoadContainer");
const easyBtn = document.getElementById("easyBtn");
const mediumBtn = document.getElementById("mediumBtn");
const hardBtn = document.getElementById("hardBtn");
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
var gameLoopConnection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var directionConnection = new signalR.HubConnectionBuilder().withUrl("/directionHub").build();
let emptyArray = [];


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

easyBtn.addEventListener("click", async () => { buttonSound("Easy", 200) });

mediumBtn.addEventListener("click", async () => { buttonSound("Medium", 150) });

hardBtn.addEventListener("click", async () => { buttonSound("Hard", 100) });

changeLevelBtn.addEventListener("click", () => {
    if (noiseOn) {
        playAudio("/assets/buttonClick.mp3");
    }
    gameOverContainer.style.display = "none";
    initLoadContainer.style.display = "flex";
});

document.addEventListener("keydown", (e) => {
    if (dead) {
        gameOverContainer.style.display = "none"
        countdown(currentLevel);
    } else {
        directionConnection.invoke("ClientDirection", e.key)
    }
    e.preventDefault();
});


async function buttonSound(level, difficulty) {
    if (noiseOn) {
        playAudio("/assets/buttonClick.mp3");
    }
    await delay(500);
    initSequence(level, difficulty)
}


function initSequence(difficulty, level) {
    initLoadContainer.style.display = "none";
    uiLevel.textContent = `Level: ${difficulty}`;
    countdown(level);
}


function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}


async function countdown(level) {
    currentLevel = level;
    dead = false;
    countdownTxt.style.display = "flex";
    updateUiScore(0);
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


function drawBoard(img) {
    let i = 0
    for (let r = 0; r < rows; r++) {
        for (let c = 0; c < cols; c++) {
            document.getElementById(`img_${r}-${c}`).src = img[i];
            i++;
        }
    }
}


function drawEmptyBoard() {
    let i = 0
    for (let r = 0; r < rows; r++) {
        for (let c = 0; c < cols; c++) {
            document.getElementById(`img_${r}-${c}`).src = "/assets/empty.svg";
            i++;
        }
    }
}


gameLoopConnection.on("UpdateUi", (list, serverScore, gameOver) => {
    updateClientUi(list, serverScore, gameOver)
    console.log(list)
});


function updateClientUi(list, serverScore, gameOver) {
    if (serverScore > previousScore && noiseOn) {
        playAudio('/assets/bite.mp3');
    }
    drawBoard(list);
    updateUiScore(serverScore)
    if (gameOver) {
        gameLoopConnection.invoke("GameOver");
        GameOver();
    }
    previousScore = serverScore;
}


function updateUiScore(score) {
    uiScore.textContent = `SCORE: ${score * 5}`;
}


function playAudio(link) {
    var audio = new Audio(link);
    audio.play();
}


async function GameOver() {
    if (noiseOn) {
        playAudio("/assets/fail.mp3");
    }
    await delay(1000);
    dead = true;
    overlay.style.display = "flex";
    gameOverContainer.style.display = "flex";
    drawEmptyBoard();
}


gameLoopConnection.start()
    .then(() => {
        console.log("GameLoopHub Connection Successful");
        drawEmptyBoard();
    })
    .catch((err) => {
        return console.error(err.toString());
    });


directionConnection.start()
    .then(() => {
        console.log("DirectionHub Connection Successful");
    })
    .catch((err) => {
        return console.error(err.toString());
    });