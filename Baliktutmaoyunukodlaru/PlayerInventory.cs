using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("UI Referansları")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI bagText;

    [Header("Ekonomi")]
    public int gold = 0;

    public List<FishData> caughtFishes = new List<FishData>();


    [Header("Çanta Ayarları")]
    public int maxCapacity = 5;

    private List<FishData> caughtFishList = new List<FishData>();



    [Header("Envanter UI Paneli")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryContentText;

    
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        UpdateInventoryText();

        Time.timeScale = 0f;
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);

        
        Time.timeScale = 1f;
    }

    private void UpdateInventoryText()
    {
        if (caughtFishes.Count == 0)
        {
            inventoryContentText.text = "Çantan şu an boş...";
            return;
        }

        Dictionary<string, int> fishCounts = new Dictionary<string, int>();

        foreach (FishData fish in caughtFishes)
        {
            if (fishCounts.ContainsKey(fish.fishName))
            {
                fishCounts[fish.fishName]++;
            }
            else
            {
                fishCounts[fish.fishName] = 1;
            }
        }

        string content = "ÇANTADAKİLER\n\n";

        foreach (var item in fishCounts)
        {
            content += item.Key + ": " + item.Value + " Adet\n";
        }

        inventoryContentText.text = content;
    }


    void Start()
    {
        UpdateUI(); 
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("PlayerGold", gold);
        PlayerPrefs.SetInt("MaxCapacity", maxCapacity);
        PlayerPrefs.Save();
    }
    public int CurrentWeight
    {
        get
        {
            int totalWeight = 0;
            foreach (var fish in caughtFishList)
            {
                totalWeight += fish.weight;
            }
            return totalWeight;
        }
    }

    public bool AddFish(FishData fish)
    {
        if (CurrentWeight + fish.weight > maxCapacity)
        {
            Debug.LogWarning("Çanta dolu! Balığı dükkanda satmalısın.");
            return false;
        }
        if (caughtFishes.Count < maxCapacity)
        {
            caughtFishes.Add(fish);

            UpdateUI();
        }
        else
        {
            Debug.Log("Çanta dolu, balık eklenemedi!");
        }

        caughtFishList.Add(fish);
        UpdateUI(); 
        return true;
    }

    public void SellAllFish()
    {
        if (caughtFishList.Count == 0) return;

        int earnedGold = 0;
        foreach (var fish in caughtFishList)
        {
            earnedGold += fish.GetDynamicPrice();
        }

        gold += earnedGold;
        caughtFishList.Clear();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "Altın: " + gold;
        }

        if (bagText != null)
        {
            bagText.text = "Çanta: " + CurrentWeight + "/" + maxCapacity;
        }
    }
}