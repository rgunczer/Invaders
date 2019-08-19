/**
 * Created by Robert Gunczer on 1/23/2016.
 * mod: 2019-08-19
 */

window.addEventListener("load", eventWindowLoad, false);

var MAX_STARS_COUNT = 205;

var player = {
    initialPosX: 243,
    initialPosY: 438,
    x: 0,
    y: 0,
    lives: 0,
    score: 0,
    active: true,
    recoveryTickCount: 100,
    willFire: false,

    reset: function () {
        this.x = this.initialPosX;
        this.y = this.initialPosY;
        this.lives = 3;
    }
};

var bulletColor = 'Yellow';
var backgroundColor = 'black';
var textInstructionToBegin = 'Press Enter or Press Fire to begin...';
var bulletWidth = 2;
var bulletHeight = 8;
var asset = {
    files: [
        'title.json',
        'player.json',
        'enemy.json',
        'live.json'
    ],
    titleData : '',
    playerData : '',
    enemyData : '',
    liveData : '',
    assetLoadCounter : 0,

    loadAll: function() {
        var len = this.files.length;
        for (var i = 0; i < len; ++i) {
            this.loadAsset(asset.files[i]);
        }
    },

    loadAsset: function (fileName) {
        var request = new XMLHttpRequest();
        request.onload = function() {
            var obj = JSON.parse(this.responseText);

            switch(fileName) {
                case 'title.json':  asset.titleData = obj.data;  break;
                case 'player.json': asset.playerData = obj.data; break;
                case 'enemy.json':  asset.enemyData = obj.data;  break;
                case 'live.json':   asset.liveData = obj.data;   break;
            }

            if (++asset.assetLoadCounter > asset.files.length - 1) {
                gameState = 'menu';

                prepareRawAssets();

                //window.addEventListener("keydown", eventKeyPressed, true);
                document.onkeydown = function(e) {
                    e = e ? e:window.event;
                    keyPressList[e.keyCode] = true;
                };

                document.onkeyup = function(e) {
                    e = e ? e:window.event;
                    keyPressList[e.keyCode] = false;
                };
            }
        };
        request.open("get", fileName, true);
        request.send();
    }
};
var canvas;
var context;
var keyPressList = [];
var tick = 0;
var gameState = 'loading';
var stars = [];
var enemies = [];
var bullets = [];
var damages = [];
var starImageId;

var enemyMovingRight = true;
var enemyAnimFlip = true;
var loadingTextIndex = 0;

var bufferCanvasEnemy;
var bufferContextEnemy;

var bufferCanvasLive;
var bufferContextLive;

var bufferCanvasPlayer;
var bufferContextPlayer;


function inputPlayerFire() {
    if (gameState === 'playing') {
        if (player.active && player.lives > 0) {
            player.willFire = true;
        }
    } else if (gameState === 'menu') {
        gameState = 'playing';
        initGame();
    } else if (gameState === 'win' || gameState === 'gameOver') {
        gameState = 'menu';
    }
}

function inputMoveLeft() {
    player.x -= 3;

    if (player.x <= 0) {
        player.x = 0;
    }
}

function inputMoveRight() {
    player.x += 3;

    if (player.x >= 492) {
        player.x = 492;
    }
}

function handleInput() {
    switch (gameState) {
        case 'menu':
            if (keyPressList[13]) { // enter
                keyPressList[13] = false;
                inputPlayerFire();
            }
            break;

        case 'playing':
            if (keyPressList[37]) { // left
                inputMoveLeft();
            }

            if (keyPressList[39]) { // right
                inputMoveRight();
            }

            if (keyPressList[32]) { // space
                keyPressList[32] = false;
                inputPlayerFire();
            }

            if (keyPressList[27]) { // esc
                gameState = 'menu';
            }

            break;

        case 'gameOver':
        case 'win':
            if (keyPressList[13]) { // enter
                keyPressList[13] = false;
                inputPlayerFire();
            }
            break;
    }
}

function eventWindowLoad() {
    canvas = document.getElementById('canvasOne');
    context = canvas.getContext('2d');

    bufferCanvasEnemy = document.createElement('canvas');
    bufferCanvasEnemy.width = 30;
    bufferCanvasEnemy.height = 30;
    bufferContextEnemy = bufferCanvasEnemy.getContext('2d');

    bufferCanvasLive = document.createElement('canvas');
    bufferCanvasLive.width = 20;
    bufferCanvasLive.height = 20;
    bufferContextLive = bufferCanvasLive.getContext('2d');

    bufferCanvasPlayer = document.createElement('canvas');
    bufferCanvasPlayer.width = 30;
    bufferCanvasPlayer.height = 30;
    bufferContextPlayer = bufferCanvasPlayer.getContext('2d');

    window.setTimeout(function() {asset.loadAll();}, 1000);

    clearScreenWithColor(backgroundColor);
    gameLoop();
}

function gameLoop() {
    window.setTimeout(gameLoop, 12);
    clearScreenWithColor(backgroundColor);
    handleInput();
    switch(gameState) {
        case 'playing':  drawPlaying();  break;
        case 'menu':     drawMenu();     break;
        case 'gameOver': drawGameOver(); break;
        case 'win':      drawYouWin();   break;
        case 'loading':  drawLoading();  break;
    }
    ++tick;
}

function drawPlaying() {
    updateEnemies();
    drawStars();
    drawRocks();
    drawEnemies();
    drawEnemyAnim();
    drawSide();
    updateAndDrawBullets();
    playerFire();
    drawLives();

    if (!player.active) {
        --player.recoveryTickCount;
        if (player.recoveryTickCount < 0) {
            player.active = true;
        }
    }

    if (player.active) {
        context.drawImage(bufferCanvasPlayer, player.x, player.y);
    }

    if (tick % 100 === 0) {
        enemyFire();
    }

    if (tick % 23 === 0) { // check win or lose
        if (player.lives <= 0) {
            gameState = 'gameOver';
        }

        if (enemies[ enemies.length - 1 ].y > 330) {
            gameState = 'gameOver';
        }

        var win = true;
        var len = enemies.length;
        var enemy;
        for (var i = 0; i < len; ++i) {
            enemy = enemies[i];
            if (enemy.active) {
                win = false;
            }
        }

        if (win) {
            gameState = 'win';
        }
    }
}

function updateEnemies() {
    if (tick % 120 === 0) {
        if (enemyMovingRight === true) {
            moveRight();
        } else {
            moveLeft();
        }
    }
}

function drawMenu() {
    drawTitle();
    drawText(textInstructionToBegin);
}

function drawLoading() {
    var texts = ['Loading   ', 'Loading.  ', 'Loading.. ', 'Loading...'];
    if (tick % 4 === 0) {
        ++loadingTextIndex;
        if (loadingTextIndex > texts.length - 1) {
            loadingTextIndex = 0;
        }
    }
    drawTextCentered(texts[loadingTextIndex]);
}

function drawGameOver() {
    context.fillStyle = 'White';
    context.font = '32px Impact';
    var text = 'Game Over';
    var metrics = context.measureText(text);
    var textWidth = metrics.width;
    var x = canvas.width / 2 - textWidth / 2;
    var y = 220; //canvas.height / 2;
    context.fillText(text, x, y);
    drawTextCentered(textInstructionToBegin);
}

function drawYouWin() {
    context.fillStyle = 'White';
    context.font = '32px Impact';
    var text = 'You Win!';
    var metrics = context.measureText(text);
    var textWidth = metrics.width;
    var x = canvas.width / 2 - textWidth / 2;
    var y = 220; //canvas.height / 2;
    context.fillText(text, x, y);
    drawTextCentered(textInstructionToBegin);
}

function initGame() {
    initEnemies();
    initStars();

    enemyMovingRight = Math.floor( Math.random() * 100 ) > 50;

    damages.length = 0;
    bullets.length = 0;

    player.reset();
}

function drawEnemies() {
    var len = enemies.length;
    var obj;
    for(var i = 0; i < len; ++i) {
        obj = enemies[i];
        if (obj.active) {
            context.drawImage(bufferCanvasEnemy, obj.x, obj.y);
        }
    }
}

function drawEnemyAnim() {
    var len = enemies.length;
    var enemy;
    var dcolor;
    var hcolor;
    var i;
    var dx = -8;
    var dy = 1;

    if (tick % 20 === 0) {
        enemyAnimFlip = !enemyAnimFlip;
    }

    if (enemyAnimFlip === true) {
        dcolor = 'blue'; //9;
        hcolor = backgroundColor; //0;
    } else {
        dcolor = backgroundColor; //0;
        hcolor = 'blue'; //9;
    }


    context.beginPath();
    for (i = 0; i < len; ++i) {
        enemy = enemies[i];
        if (enemy.active) {
            // right
            context.moveTo(enemy.x + 1 + 27 + dx, enemy.y + 2 + 17 + dy); context.lineTo(enemy.x + 3 + 27 + dx, enemy.y + 2 + 17 + dy);
            context.moveTo(enemy.x + 4 + 27 + dx, enemy.y + 2 + 17 + dy); context.lineTo(enemy.x + 4 + 27 + dx, enemy.y + 5 + 17 + dy);

            // left
            context.moveTo(enemy.x - 9 + 27 + dx, enemy.y + 2 + 17 + dy); context.lineTo(enemy.x - 12 + 27 + dx, enemy.y + 2 + 17 + dy);
            context.moveTo(enemy.x - 12 + 27 + dx, enemy.y + 2 + 17 + dy); context.lineTo(enemy.x - 12 + 27 + dx, enemy.y + 5 + 17 + dy);
        }
    }
    context.strokeStyle = dcolor;
    context.closePath();
    context.stroke();

    context.beginPath();
    for (i = 0; i < len; ++i) {
        enemy = enemies[i];
        if (enemy.active) {
            // right
            context.moveTo(enemy.x + 2 + 27 + dx, enemy.y + 1 + 17 + dy); context.lineTo(enemy.x + 4 + 27 + dx, enemy.y + 1 + 17 + dy);
            context.moveTo(enemy.x + 5 + 27 + dx, enemy.y + 1 + 17 + dy); context.lineTo(enemy.x + 5 + 27 + dx, enemy.y + 4 + 17 + dy);

            // left
            context.moveTo(enemy.x - 10 + 27 + dx, enemy.y + 1 + 17 + dy); context.lineTo(enemy.x - 13 + 27 + dx, enemy.y + 1 + 17 + dy);
            context.moveTo(enemy.x - 13 + 27 + dx, enemy.y + 2 + 17 + dy); context.lineTo(enemy.x - 13 + 27 + dx, enemy.y + 4 + 17 + dy);
        }
    }
    context.strokeStyle = hcolor;
    context.closePath();
    context.stroke();
}

function drawLiveFromRawAsset() {
    bufferContextLive.fillStyle = backgroundColor;
    bufferContextLive.fillRect(0, 0, bufferCanvasLive.width, bufferCanvasLive.height);

    bufferContextLive.fillStyle = 'White';

    var xStart = 0;
    var yStart = 0;
    var x = xStart;
    var y = yStart;
    var scale = 1;
    var str = asset.liveData;
    var i;
    var length = str.length;
    var ch;

    for (i = 0; i < length; ++i) {
        ch = str.charAt(i);
        y += scale;

        if (ch === '1') {
            bufferContextLive.fillRect(x, y, scale, scale);
        } else if (ch === '2') {
            y = yStart;
            x += scale;
        } else if (ch === '0') {

        } else {
            console.log('unknown color in live: ' + ch);
        }
    }
}

function drawEnemyFromRawAsset() {
    bufferContextEnemy.fillStyle = backgroundColor;
    bufferContextEnemy.fillRect(0, 0, bufferCanvasEnemy.width, bufferCanvasEnemy.height);

    var xStart = 0;
    var yStart = 0;
    var x = xStart;
    var y = yStart;
    var scale = 1;
    var str = asset.enemyData;
    var i;
    var length = str.length;
    var ch;

    for (i = 0; i < length; ++i) {
        ch = str.charAt(i);

        y += scale;

        if (ch === '2') {
            y = yStart;
            x += scale;
        } else if (ch === '1') {
            bufferContextEnemy.beginPath();
            bufferContextEnemy.fillStyle = 'Indigo';
            bufferContextEnemy.fillRect(x, y, scale, scale);
            bufferContextEnemy.closePath();
        } else if (ch === '9') {
            bufferContextEnemy.beginPath();
            bufferContextEnemy.fillStyle = 'Blue';
            bufferContextEnemy.fillRect(x, y, scale, scale);
            bufferContextEnemy.closePath();
        } else if (ch === '3') {
            bufferContextEnemy.beginPath();
            bufferContextEnemy.fillStyle = 'LightGray';
            bufferContextEnemy.fillRect(x, y, scale, scale);
            bufferContextEnemy.closePath();
        } else if (ch === '0') {
            // do nothing (should be transparent pixel)
        } else {
            console.log('Unknown color in enemy: ' + ch);
        }
    }
}

function drawPlayerFromRawAsset() {
    bufferContextPlayer.fillStyle = backgroundColor;
    bufferContextPlayer.fillRect(0, 0, bufferCanvasPlayer.width, bufferCanvasPlayer.height);

    bufferContextPlayer.fillStyle = 'White';

    var xStart = 0;
    var yStart = 0;
    var x = xStart;
    var y = yStart;
    var scale = 1;
    var str = asset.playerData;
    var i;
    var length = str.length;
    var ch;

    bufferContextPlayer.beginPath();
    for (i = 0; i < length; ++i) {
        ch = str.charAt(i);
        y += scale;

        if (ch === '1') {
            bufferContextPlayer.rect(x, y, scale, scale);
        } else if (ch === '2') {
            y = yStart;
            x += scale;
        } else if (ch === '0') {

        } else {
            console.log('unknown color in player: ' + ch);
        }

    }
    bufferContextPlayer.closePath();
    bufferContextPlayer.fill();
}

function drawTitle() {
    var yStart = 170; //170;
    var x = 70;
    var y = yStart;
    var scale = 1;
    var str = asset.titleData;
    var i;
    var length = str.length;
    var ch;
    context.beginPath();
    for (i = 0; i < length; ++i) {
        ch = str.charAt(i);

        if (y > 259) {
            y += scale + 1;
        } else {
            y += scale;
        }

        if (ch === '1') {
            context.rect(x, y, scale, scale);
        } else if (ch === '2') {
            y = yStart;
            x += scale;
        } else if (ch === '0') {
            // do nothing
        } else {
            console.log('Unknown color in title: ' + ch);
        }
    }
    context.closePath();
    context.fillStyle = 'LightGray';
    context.fill();
}

function clearScreenWithColor(color) {
    context.fillStyle = color;
    context.fillRect(0, 0, canvas.width, canvas.height);
}

function prepareRawAssets() {
    drawEnemyFromRawAsset();
    drawPlayerFromRawAsset();
    drawLiveFromRawAsset();
}

function playerFire() {
    if (player.willFire === true && tick % 15 === 0) {
        var bullet = {};
        bullet.x = player.x + 15;
        bullet.y = player.y - 10;
        bullet.step = -2;
        bullet.active = true;
        bullets.push(bullet);
        player.willFire = false;
    }
}

function isEnemyHit(x, y) {
    var enemy;
    var w = 30;
    var h = 20;
    var len = enemies.length;
    for(var i = 0; i < len; ++i) {
        enemy = enemies[i];
        if (enemy.active && (x > enemy.x && x < enemy.x + w) &&
                            (y > enemy.y && y < enemy.y + h)) {
            return enemy;
        }
    }
    return null;
}

function isPlayerHit(x, y) {
    var w = 30;
    var h = 30;
    return (player.active && (x > player.x && x < player.x + w) &&
                             (y > player.y && y < player.y + h));
}

function isRockHit(x, y) {
    var data = context.getImageData(x, y, 1, 1).data;
    var r = data[0];
    var g = data[1];
    var b = data[2];

    return (r === 128 && g === 128 && b === 128);
}

function updateAndDrawBullets() {
    context.fillStyle = bulletColor;
    context.beginPath();
    var damage = {};
    var bullet;
    var len = bullets.length;
    for(var i = 0; i < len; ++i) {
        bullet = bullets[i];
        if (bullet.active) {
            bullet.y += bullet.step;

            if (bullet.y < 0 || bullet.y > canvas.height) {
                bullet.active = false;
            } else {
                if (tick % 2 === 0) {
                    if (bullet.step > 0) { // enemy bullet

                        if (bullet.y > 360 && bullet.y < 420) { // check collision with rocks
                            if (isRockHit(bullet.x, bullet.y, i)) {
                                bullet.active = false;
                                damage.x = bullet.x - 1;
                                damage.y = bullet.y - 5;
                                damages.push(damage);
                            }
                        }

                        if (bullet.y > 430) { // check collision with player
                            if (player.lives > 0 && isPlayerHit(bullet.x, bullet.y)) {
                                bullet.active = false;
                                player.active = false;
                                --player.lives;

                                if (player.lives > 0) {
                                    player.recoveryTickCount = 100;
                                }
                            }
                        }

                        if (bullet.active) {
                            context.rect(bullet.x, bullet.y, bulletWidth, bulletHeight);
                        }
                    } else { // player bullet
                        var enemy = isEnemyHit(bullet.x, bullet.y);
                        if (enemy != null) {
                            enemy.active = false;
                            bullet.active = false;
                            ++player.score;
                        } else if (isRockHit(bullet.x, bullet.y, i)) {
                            bullet.active = false;
                            damage.x = bullet.x - 1;
                            damage.y = bullet.y - 5;
                            damages.push(damage);
                        } else {
                            context.rect(bullet.x, bullet.y, bulletWidth, bulletHeight);
                        }
                    }
                } else {
                    context.rect(bullet.x, bullet.y, bulletWidth, bulletHeight);
                }
            }
        }
    }
    context.closePath();
    context.fill();

    for(i = 0; i < len; ++i) {
        bullet = bullets[i];
        if (!bullet.active) {
            bullets.splice(i, 1);
            return;
        }
    }
}

function moveDown() {
    var len = enemies.length;
    for(var i = 0; i < len; ++i) {
        enemies[i].y += 7;
    }
}

function moveRight() {
    var len = enemies.length;

    for(var i = 0; i < len; ++i) {
        if (enemies[53].x < 480) {
            enemies[i].x += 7;
        } else {
            enemyMovingRight = false;
            moveDown();
            return;
        }
    }
}

function moveLeft() {
    var len = enemies.length;
    for(var i = 0; i < len; ++i) {
        if (enemies[53].x > 330) {
            enemies[i].x -= 7;
        } else {
            enemyMovingRight = true;
            moveDown();
            return;
        }
    }
}

function drawLives() {
    context.beginPath();
    context.rect(canvas.width - 92, canvas.height - 100, 80, 50);
    context.closePath();
    context.fillStyle = backgroundColor;
    context.fill();

    var x = canvas.width - 86;
    var y = canvas.height - 80;
    for(var i = 0; i < player.lives; ++i) {
        context.drawImage(bufferCanvasLive, x, y);
        x += 23;
    }

    context.beginPath();
    context.rect(canvas.width - 92, 80, 80, 51);
    context.closePath();
    context.fillStyle = backgroundColor;
    context.fill();

    context.fillStyle = 'White';
    context.font = '20px Impact';
    context.fillText('Score:', 553, 100);
    context.fillText('' + player.score, 553, 125);
}

function drawTextAt(x, y, text) {
    context.fillStyle = 'Yellow';
    context.font = '12px Courier New';
    context.fillText(text, x, y);
}

function drawTextCentered(text) {
    context.fillStyle = 'Orange';
    context.font = '14px Courier New';

    var metrics = context.measureText(text);
    var textWidth = metrics.width;
    var x = canvas.width / 2 - textWidth / 2;
    var y = canvas.height / 2;
    context.fillText(text, x, y);
}

function drawText(text) {
    context.fillStyle = 'Orange';
    context.font = '14px Courier New';

    var metrics = context.measureText(text);
    var textWidth = metrics.width;
    var x = canvas.width / 2 - textWidth / 2;
    var y = canvas.height / 2 + 50;
    context.fillText(text, x, y);
}

function drawAxes() {
    // draw x axis
    context.beginPath();
    context.strokeStyle = 'Red';
    context.moveTo(0, canvas.height/2);
    context.lineTo(canvas.width, canvas.height/2);
    context.stroke();

    // draw y axis
    context.beginPath();
    context.strokeStyle = 'Yellow';
    context.moveTo(canvas.width/2, 0);
    context.lineTo(canvas.width/2, canvas.height);
    context.stroke();
}


function drawRocks() {
    //drawAxes();
    var originX = canvas.width / 2;

    var shiftX = -50;
    var y = 380;
    drawRockAt(originX - (originX / 2) + shiftX, y);
    drawRockAt(originX + shiftX, y);
    drawRockAt(originX + (originX / 2) + shiftX, y);

    var len = damages.length;
    var damage;
    context.beginPath();
    for (var i = 0; i < len; ++i) {
        damage = damages[i];
        context.rect(damage.x-6, damage.y, 12, 18);
    }
    context.closePath();
    context.fillStyle = backgroundColor;
    context.fill();
}

function drawRockAt(originX, originY) {
    var diff = 20;
    var h = 35;
    var size = 12;

    var x = originX + diff;
    var y = originY;

    // left triangle
    context.fillStyle = 'gray';
    context.beginPath();
    context.moveTo(x, y);
    context.lineTo(x, y-size);
    context.lineTo(x+size, y);
    context.lineTo(x, y);

    x = originX - diff;

    // right triangle
    context.moveTo(x, y);
    context.lineTo(x, y-size);
    context.lineTo(x-size, y);
    context.lineTo(x, y);

    x = originX;
    y = originY;
    context.rect(x-((diff*2)/2), y-size, diff*2, size);

    var w = 2*diff+2*size;
    context.rect(x-w/2, y, w, h);



    context.closePath();
    context.fill();


    context.beginPath();
    context.arc(x, y+35, 12, 0, 2*Math.PI);
    context.closePath();
    context.fillStyle = backgroundColor;
    context.fill();
}

function initEnemies() {
    var xStart = 90;
    var yStart = 30;

    var xPos = xStart;
    var yPos = yStart;

    enemies.length = 0;

    for(var row = 0; row < 6; ++row) {
        xPos = xStart;
        for(var col = 0; col < 9; ++col) {
            var obj = {};
            obj.x = xPos;
            obj.y = yPos;
            obj.active = true;
            enemies.push(obj);
            xPos += 40;
        }
        yPos+= 30;
    }
}

function initStars() {
    var x;
    var y;
    stars.length = 0;

    for (var i = 0; i < MAX_STARS_COUNT; ++i) {
        x = Math.floor( (Math.random() * (canvas.width - 100) ) + 1);
        y = Math.floor( (Math.random() * canvas.height) + 1);
        var obj = {};
        obj.x = x;
        obj.y = y;
        stars.push(obj);
    }

    var r = 160;
    var g = 160;
    var b = 160;
    var a = 255;

    starImageId = context.createImageData(1, 1);
    var d = starImageId.data;
    d[0] = r;
    d[1] = g;
    d[2] = b;
    d[3] = a;
}

function drawStars() {
    var obj;
    for (var i = 0; i < MAX_STARS_COUNT; ++i) {
        obj = stars[i];
        context.putImageData(starImageId, obj.x, obj.y);
    }
}

function drawSide() {
    var kx = canvas.width;
    var ky = canvas.height;

    context.fillStyle = '#333333';
    context.beginPath();
    context.rect(536, 0, kx, ky);
    context.closePath();
    context.fill();

    context.strokeStyle = 'Gray';
    context.beginPath();
    context.rect(536, 0, kx, ky);
    context.closePath();
    context.stroke();

    context.strokeStyle = 'Gray';
    context.beginPath();
    context.moveTo(530, 0);
    context.lineTo(530, ky);
    context.moveTo(532, 0);
    context.lineTo(532, ky);
    context.closePath();
    context.stroke();

    context.strokeStyle = 'LightGray';
    context.beginPath();
    context.moveTo(531, 0);
    context.lineTo(531, ky);
    context.closePath();
    context.stroke();
}

function enemyFire() {
    var len = enemies.length - 1;
    var i;
    var enemy;
    for(i = len; i > -1; --i) {
        enemy = enemies[i];
        if (enemy.active && enemy.x > player.x - 33 &&
                            enemy.x < player.x + 33) {
            var bullet = {};
            bullet.x = enemy.x + 15;
            bullet.y = enemy.y + 20;
            bullet.step = 1;
            bullet.active = true;
            bullets.push(bullet);
            return;
        }
    }
}
