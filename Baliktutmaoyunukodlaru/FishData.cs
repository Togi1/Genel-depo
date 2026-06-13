using UnityEngine;

[CreateAssetMenu(fileName = "YeniBalik", menuName = "PixelAngler/Balik Verisi")]
public class FishData : ScriptableObject
{
    public string fishName = "Yeni Balık";
    public Sprite fishIcon;
    public int weight = 1;

    [Header("Ekonomi ve Zorluk")]
    [Tooltip("Bu temel fiyattır. Satış fiyatı zorlukla çarpılacak.")]
    public int baseGoldValue = 10;

    [Tooltip("Yakalanma zorluğu (1 en kolay, 10 en zor)")]
    [Range(1f, 10f)]
    public float catchDifficulty = 1f;

    [Tooltip("Çıkma ihtimali ağırlığı (Örn: Sazan için 100, Efsanevi balık için 5)")]
    public int spawnChance = 100;

    public int GetDynamicPrice()
    {
        return Mathf.RoundToInt(baseGoldValue * catchDifficulty);
    }
}