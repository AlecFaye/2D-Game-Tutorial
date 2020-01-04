using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;
    public int currentItemSelected;
    public int currentItemPrice;

    private Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<Player>();

            if (player != null)
            {
                UIManager.Instance.OpenShop(player.diamonds);
                UIManager.Instance.selectionImg.enabled = false;
                shopPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player != null)
            {
                UIManager.Instance.OpenShop(player.diamonds);
                shopPanel.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player != null)
            {
                UIManager.Instance.OpenShop(player.diamonds);
            }
        }
    }

    public void SelectItem(int item)
    {
        // 0 = flame sword
        // 1 = boots of flight
        // 2 = castle key

        switch (item)
        {
            case 0: // Flame Sword
                UIManager.Instance.UpdateShopSelection(80);
                currentItemSelected = 0;
                currentItemPrice = 200;
                break;

            case 1: // Boots of Flight
                UIManager.Instance.UpdateShopSelection(-25);
                currentItemSelected = 1;
                currentItemPrice = 400;
                break;

            case 2: // Castle Key
                UIManager.Instance.UpdateShopSelection(-130);
                currentItemSelected = 2;
                currentItemPrice = 100;
                break;
        }
    }

    public void BuyItem()
    {
        if (player.diamonds >= currentItemPrice) // If the player has enough gems to buy the item
        {
            // Subtract gems from total player gems
            // Give item to player
            player.diamonds -= currentItemPrice;

            if (currentItemSelected == 2)
            {
                GameManager.Instance.HasCastleKey = true;
            }
        }
        else
        {
            // Cancel sale by closing shop
            shopPanel.SetActive(false);
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
