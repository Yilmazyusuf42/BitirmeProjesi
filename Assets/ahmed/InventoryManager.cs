using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;
    // Start is called before the first frame update
    void Start()
    {
        itemSlot.ToList().ForEach(x => x.GetComponent<Button>().onClick.AddListener(() => { DeActivateOthers(x.GetComponent<Button>()); }));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }


    public void AddItem(string itemName, int quantity, Sprite itemsprite)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemsprite);
                return;
            }
        }
    }

    public void DeselectAllSlot()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;

        }
    }


    public void DeActivateOthers(Button button)
    {
        itemSlot.ToList().ForEach(x =>
        {
            if (x.GetComponent<Button>() == button)
            {
                x.gameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                x.gameObject.GetComponent<Image>().color = Color.gray;
            }
        });
    }
}
