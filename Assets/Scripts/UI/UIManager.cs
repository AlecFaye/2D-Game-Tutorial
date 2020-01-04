using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerGemCountText;
    public Image selectionImg;
    public Text gemCountText;
    public Image[] life_points;

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is null.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void OpenShop(int gemCount)
    {
        playerGemCountText.text = "" + gemCount + "G";
    }

    public void UpdateShopSelection(int yPos)
    {
        selectionImg.enabled = true;
        selectionImg.rectTransform.anchoredPosition = new Vector2(selectionImg.rectTransform.anchoredPosition.x, yPos);
    }

    public void UpdateGemCount(int diamonds)
    {
        gemCountText.text = "" + diamonds;
    }

    public void UpdateLives(int livesRemaining)
    {
        // Loops through the remaining number of lives to display
        for (int i = 0; i < life_points.Length; i++)
        {
            if (i == livesRemaining)
            {
                life_points[i].enabled = false;
            }
        }
    }
}
