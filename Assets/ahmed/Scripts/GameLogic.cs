using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameLogic {

  

    private static void CreateSprite(Sprite sprite)
    {
        UtilsClass.CreateWorldSprite("Sprite", sprite,Vector3.zero, new Vector3(20,20), 0, Color.white);

    }
   
}
