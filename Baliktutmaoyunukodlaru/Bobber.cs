using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFloating = false;
    public bool fishIsBiting = false; 

    [Header("Balık Vurma Ayarları")]
    public float minWaitTime = 2f; 
    public float maxWaitTime = 5f; 
    public GameObject exclamationMark; 

    [Header("Balık Havuzu")]
    public List<FishData> fishPool;






    void Update()
    {
        if (fishIsBiting && Input.GetMouseButtonDown(0))
        {
            FishingMiniGame miniGame = FindFirstObjectByType<FishingMiniGame>();

            if (miniGame != null && fishPool != null && fishPool.Count > 0)
            {
                int totalWeight = 0;
                foreach (var f in fishPool)
                {
                    totalWeight += f.spawnChance;
                }

                int randomValue = Random.Range(0, totalWeight);
                FishData selectedFish = fishPool[0]; 

                int currentWeight = 0;
                foreach (var f in fishPool)
                {
                    currentWeight += f.spawnChance;
                    if (randomValue < currentWeight)
                    {
                        selectedFish = f;
                        break;
                    }
                }

                miniGame.StartMiniGame(selectedFish);
            }

            Destroy(gameObject);
        }
    }




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (exclamationMark != null)
        {
            exclamationMark.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !isFloating)
        {
            StartFloating();
        }
    }

    void StartFloating()
    {
        isFloating = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        Debug.Log("Şamandıra suya yerleşti! Balık bekleniyor...");

        StartCoroutine(WaitForBite());
    }

    IEnumerator WaitForBite()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        yield return new WaitForSeconds(waitTime);

        fishIsBiting = true;
        Debug.Log("Balık Vurdu! Hemen Çek!");

        if (exclamationMark != null)
        {
            exclamationMark.SetActive(true);
        }

        transform.position = new Vector2(transform.position.x, transform.position.y - 0.2f);
    }
}