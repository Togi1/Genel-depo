using UnityEngine;

public class FishingMechanic : MonoBehaviour
{
    [Header("Şamandıra Ayarları")]
    public GameObject bobberPrefab; 
    public Transform castPoint; 
    public float throwForce = 7f; 

    private GameObject currentBobber;

    void Update()
    {
        FishingMiniGame miniGame = FindFirstObjectByType<FishingMiniGame>();

        if (miniGame != null && miniGame.miniGamePanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            CastRod();
        }
    }

    void CastRod()
    {
        if (currentBobber != null) return;
        currentBobber = Instantiate(bobberPrefab, castPoint.position, Quaternion.identity);

        Rigidbody2D rb = currentBobber.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 throwDirection = new Vector2(1f, 1f).normalized;
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }
    }
}