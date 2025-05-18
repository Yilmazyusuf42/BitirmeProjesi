using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
   private static GameAssets _i;
   public static GameAssets i 
   {
    get {
        if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
        return _i;
    }
   }
   

    

    public Sprite s_Armor_1;
    public Sprite s_Armor_2;
    public Sprite s_Armor_None;

    public Sprite s_Helmet_1;
    public Sprite s_Helmet_None;

    public Sprite s_Health_Potion;

    public Sprite s_Sword_1;
    public Sprite s_Sword_2;

}
