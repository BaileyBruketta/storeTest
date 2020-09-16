using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour
{
    // Start is called before the first frame update
    
    //pieces of Hud
    public GameObject TimeOfDayTextObject;
    public GameObject StoreItemSelectionMenu;
    public GameObject CartObject;
    public GameObject PurchaseMenu;
    public ItemPanel[] StorePanels;
    public ItemPanel[] CartPanels;

    public float CartTotalNum;
    public Text CartTotal;
    public Text AvailableFunds;
    public float AvailableFundsNum;

    public TextAsset ItemCatalogue;
    public TextAsset ItemsInStock;

    public Cart CartDevice;

    public GameManager GameManagerd;
    void Start()
    {
        EnableTimeOfDayUI();
        DisableStoreItemSelectionMenu();
        DisableCartObject();
        DisablePurchaseMenu();
        DrawAvailableFunds();
        AvailableFundsNum = GameManagerd.availableFunds;
        Cursor.visible=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawAvailableFunds()
    {
        AvailableFundsNum = GameManagerd.availableFunds;
        AvailableFunds.text = "Available: $" + GameManagerd.availableFunds.ToString();
    }

    public void RenderStoreMenu(int itemType)
    {
        DisableTimeOfdayUI();
        EnableStoreItemSelectionMenu();
        EnableCartObject();
        LoadItemsIntoPanels(itemType);
        refreshCart();
        Cursor.visible = true;

    }
    public void RenderPurchaseMenu()
    {
        EnableCartObject();
        DisableStoreItemSelectionMenu();
        EnablePurchaseMenu();
        DisableTimeOfdayUI();
        RefreshPurchaseMenu();
        Cursor.visible = true;
    }
    public void RefreshPurchaseMenu()
    {
        DrawAvailableFunds();
        CalculateCartTotal();
        refreshCart();
    }
    public void CloseStoreMenu()
    {
        DisableCartObject();
        DisableStoreItemSelectionMenu();
        EnableTimeOfDayUI();
        DisablePurchaseMenu();
        Cursor.visible = false;
    }
    public void YesBuy()
    {
        if(CartTotalNum < AvailableFundsNum)
        {
            purchaseCart();
        }
    }

    public void purchaseCart()
    {
        refreshCart();
        GameManagerd.availableFunds -= CartTotalNum;
        CartTotalNum = 0;
        DrawAvailableFunds();
        int x = CartDevice.CartItems.Count;
        for (int i = 0; i <=x; i++)
        {
            CartDevice.CartItems.RemoveAt(0);
            refreshCart();
        }

        refreshCart();
        DrawAvailableFunds();
        CalculateCartTotal();
    }
    public void NoBuy()
    {
        CloseStoreMenu();
        GameManagerd.paused = false;
        GameManagerd.pauseint += 1;
    }
    public void LoadItemsIntoPanels(int itemType)
    {
        RefreshAllStorePanels();
        
        string[] linesInFile = ItemsInStock.text.Split('\n');
        string[] ItemsCatalogueText = ItemCatalogue.text.Split('\n');

        int assigned = 0;
        for (int i = 1; i < linesInFile.Length; i++)
        {
            string[] LineSplit = linesInFile[i].Split(',');
            string[] parse = ItemsCatalogueText[int.Parse(LineSplit[0])].Split(',');
            if (int.Parse(parse[1]) == itemType)
            {
                StorePanels[assigned].ItemID = int.Parse(LineSplit[0]);
                assigned += 1;
            }
        }

        
        for (int f = 0; f< StorePanels.Length; f++)
        {
            if (StorePanels[f].ItemID != 0)
            {
               string[] parsedCatalogue = ItemsCatalogueText[StorePanels[f].ItemID].Split(',');
               StorePanels[f].ItemPrice.text = parsedCatalogue[2];
               StorePanels[f].ItemName.text = parsedCatalogue[3];
               StorePanels[f].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }

        }

    }
    public void StoreButtonClicked(int ButtonNumber)
    {

        if (StorePanels[ButtonNumber].ItemID != 0)
        {
            //find item
            StoreItem newItem = new StoreItem();
            string[] ItemsCatalogueText = ItemCatalogue.text.Split('\n');
            string[] parsedCatalogue = ItemsCatalogueText[StorePanels[ButtonNumber].ItemID].Split(',');
            newItem.ItemID = int.Parse(parsedCatalogue[0]);
            newItem.ItemType = parsedCatalogue[1];
            newItem.price = float.Parse(parsedCatalogue[2]);
            newItem.name = parsedCatalogue[3];


            int z = CartDevice.AddItem(newItem);

            //if z == 1, then remove item from store inventory
        }
        refreshCart();
    }

    public void CartButtonClicked(int ButtonNumber)
    {
        if(ButtonNumber <= CartDevice.CartItems.Count)
        {
            CartDevice.CartItems.RemoveAt(ButtonNumber);
        }
        refreshCart();
    }
    public void refreshCart()
    {
        emptyCart();
        for (int i = 0; i < CartDevice.CartItems.Count; i ++)
        {
            CartPanels[i].ItemName.text = CartDevice.CartItems[i].name;
            CartPanels[i].ItemPrice.text = CartDevice.CartItems[i].price.ToString();
            CartPanels[i].ItemID = CartDevice.CartItems[i].ItemID;
            if(CartPanels[i].ItemID!= 0) { CartPanels[i].GetComponent<Image>().color = new Color(1, 1, 1, 1); }
        }
        CalculateCartTotal();


    }

    public void CalculateCartTotal()
    {
        float pricec = 0;
        for (int i = 0; i < CartDevice.CartItems.Count; i ++)
        {
            pricec += CartDevice.CartItems[i].price;
        }
        string newText = "Cart Price: $" + pricec.ToString();
        CartTotalNum = pricec;
        CartTotal.text = newText;
    }
    public void emptyCart()
    {
        for (int i = 0; i < CartPanels.Length; i++)
        {
            CartPanels[i].itemImage = null;
            CartPanels[i].ItemID = 0;
            CartPanels[i].ItemName.text = "";
            CartPanels[i].ItemPrice.text = "";
            CartPanels[i].GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
        }
    }
    public void DisableTimeOfdayUI()
    {
        TimeOfDayTextObject.SetActive(false);
    }
    public void EnableTimeOfDayUI()
    {
        TimeOfDayTextObject.SetActive(true);
    }
    public void DisableStoreItemSelectionMenu()
    {
        StoreItemSelectionMenu.SetActive(false);
    }
    public void EnableStoreItemSelectionMenu()
    {
        StoreItemSelectionMenu.SetActive(true);
    }
    public void EnableCartObject()
    {
        CartObject.SetActive(true);
    }
    public void DisableCartObject()
    {
        CartObject.SetActive(false);
    }
    public void RefreshAllStorePanels()
    {
        for (int i = 0; i < StorePanels.Length; i ++)
        {
            StorePanels[i].itemImage = null;
            StorePanels[i].ItemID = 0;
            StorePanels[i].ItemName.text = "";
            StorePanels[i].ItemPrice.text = "";
            StorePanels[i].GetComponent<Image>().color = new Color(0,0,0,0);

        }
    }
    public void EnablePurchaseMenu()
    {
        PurchaseMenu.SetActive(true);
    }
    public void DisablePurchaseMenu()
    {
        PurchaseMenu.SetActive(false);
    }
}
