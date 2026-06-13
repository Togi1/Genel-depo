using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("UI Referansları")]
    public GameObject shopPanel;
    public TextMeshProUGUI upgradeBagText;
    public TextMeshProUGUI upgradeRodText;
    public FishingMiniGame miniGame; 

    [Header("Çanta Ayarları")]
    public int bagUpgradeCost = 50;
    public int bagUpgradeAmount = 5;
    public float costMultiplier = 1.5f;

    [Header("Olta Ayarları")]
    public int rodUpgradeCost = 100;
    public float rodUpgradeAmount = 30f; 

    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        shopPanel.SetActive(false);
        UpdateShopUI();
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateShopUI();
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SellAllFishButton()
    {
        if (playerInventory != null)
        {
            playerInventory.SellAllFish();
            playerInventory.UpdateUI();
        }
    }

    public void UpgradeBagButton()
    {
        if (playerInventory == null) return;

        if (playerInventory.gold >= bagUpgradeCost)
        {
            playerInventory.gold -= bagUpgradeCost;
            playerInventory.maxCapacity += bagUpgradeAmount;
            bagUpgradeCost = Mathf.RoundToInt(bagUpgradeCost * costMultiplier);
            playerInventory.SaveGame();
            UpdateShopUI();
            playerInventory.UpdateUI();
        }
    }

    public void UpgradeRodButton()
    {
        if (playerInventory == null || miniGame == null) return;

        if (playerInventory.gold >= rodUpgradeCost)
        {
            playerInventory.gold -= rodUpgradeCost;

            Vector2 currentSize = miniGame.catchArea.sizeDelta;
            miniGame.catchArea.sizeDelta = new Vector2(currentSize.x, currentSize.y + rodUpgradeAmount);

            rodUpgradeCost = Mathf.RoundToInt(rodUpgradeCost * costMultiplier);
            playerInventory.SaveGame();
            UpdateShopUI();
            playerInventory.UpdateUI();
        }
    }

    private void UpdateShopUI()
    {
        if (upgradeBagText != null)
        {
            upgradeBagText.text = "Çantayı Büyüt (" + bagUpgradeCost + " Altın)";
        }

        if (upgradeRodText != null)
        {
            upgradeRodText.text = "Oltayı Geliştir (" + rodUpgradeCost + " Altın)";
        }
    }
}