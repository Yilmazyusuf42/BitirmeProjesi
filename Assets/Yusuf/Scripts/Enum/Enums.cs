using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    north,
    east,
    south,
    west,
    none
}


public enum GameStates
{
    gameStarted,
    playingLevel,
    engagingEnemies,
    bossStage,
    engagingBoss,
    levelCompleted,
    gameWon,
    gameLost,
    gamePaused,
    dungeonOverviewMap,
    restartGame
}
