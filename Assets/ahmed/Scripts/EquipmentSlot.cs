using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
   public string itemName;
   public int quantity;
   public Sprite itemSprite;
   public bool isFull;
   public string itemDescription;
   public Sprite emptySprite;
   public ItemType itemType;

   [SerializeField] private TMP_Text quantityText;
   [SerializeField] private Image itemImage;

   public Image itemDescriptionImage;

  

   public GameObject selectedShader;
   public bool thisItemSelected;
   private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite,string itemDescription, ItemType itemType)
   {
    Debug.Log("LKSDMLS");
      this.itemName = itemName;
      this.quantity = quantity;
      this.itemSprite = itemSprite;
      this.itemDescription = itemDescription;
      isFull = true;
      this.itemType = itemType;

      //quantityText.text = quantity.ToString();
     // quantityText.enabled = true;
      itemImage.sprite = itemSprite;
   }

   public void OnPointerClick(PointerEventData eventData)
   {

      if (eventData.button == PointerEventData.InputButton.Left)
      {
         OnLeftClick();
      }
      if (eventData.button == PointerEventData.InputButton.Right)
      {
         OnRightClick();
      }
      Debug.Log("kljmimlkmlkmklmlkmkm");
    }

   public void OnLeftClick()
   {
      inventoryManager.DeselectAllSlot();
      //selectedShader.SetActive(true);
      thisItemSelected = true;
     
    }
    public void OnRightClick()
    {

    }
}
