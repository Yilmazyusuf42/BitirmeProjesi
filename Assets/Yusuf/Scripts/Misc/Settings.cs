using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{

    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion


    #region ROOM SETTINGS
    public const int maxChildCorridors = 3; // Bir odadan çıkabilecek maksimum koridor sayısı (3 önerilmez, çünkü odaların uyumsuzluğu 
    // nedeniyle zindan oluşturma başarısız olabilir)


    #endregion
}
