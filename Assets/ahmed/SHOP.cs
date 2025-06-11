using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHOP : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject shopMenu;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ShopMenu"))
            shop();
    }

    void shop()
    {
         if (shopMenu.activeSelf)
        {
            Time.timeScale = 1;
            shopMenu.SetActive(false);

        }
        else
        {
            Time.timeScale = 0;
            shopMenu.SetActive(true);
            

        }
    }
}
