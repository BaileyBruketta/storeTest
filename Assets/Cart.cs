using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    // Start is called before the first frame update
    public List<StoreItem> CartItems;
    public int CartMax;
    public int ItemsInCart;
    void Start()
    {
        CartMax = 5;
        CartItems = new List<StoreItem>();
        ItemsInCart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int AddItem(StoreItem item)
    {
        int returnvalue = 0;
        if (CartItems.Count < CartMax)
        {
            CartItems.Add(item);
            returnvalue = 1;
            ItemsInCart += 1;
        }
        
        return returnvalue;
    }
}

public struct StoreItem
{
    public int ItemID;
    public string ItemType;
    public float price;
    public string name;

}
