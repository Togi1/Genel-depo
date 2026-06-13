using UnityEngine;
using UnityEngine.UI;

public class FishingMiniGame : MonoBehaviour
{
    [Header("UI Referansları")]
    public GameObject miniGamePanel;
    public RectTransform fishIcon;
    public RectTransform catchArea;
    public Slider progressBar;
    private FishData currentFishToCatch;

    [Header("Dengeleme Değerleri (Game Balancing)")]
    public float catchAreaSpeed = 500f;
    public float fishSpeed = 200f; 
    public float fillSpeed = 15f;
    public float loseSpeed = 5f; 

    private float fishPosition;
    private float catchAreaPosition;
    private float fishDestination;

    private float topBoundary = 600f;
    private float bottomBoundary = 0f;

    void Start()
    {
        miniGamePanel.SetActive(false);
    }

    public void StartMiniGame(FishData fish)
    {
        currentFishToCatch = fish; 
        fishIcon.GetComponent<Image>().sprite = fish.fishIcon;
        miniGamePanel.SetActive(true);
        progressBar.value = 0f;
        fishPosition = bottomBoundary;
        catchAreaPosition = bottomBoundary;
        fishDestination = Random.Range(bottomBoundary, topBoundary);

        fishSpeed = 200f * fish.catchDifficulty;
        Debug.Log($"Şu an yakalanan: {fish.fishName} | Zorluk Çarpanı: {fish.catchDifficulty}");
    }

    void Update()
    {
        if (!miniGamePanel.activeSelf) return; 
        FishMovement();
        CatchAreaMovement();
        CheckProgress();
    }

    void FishMovement()
    {
        fishPosition = Mathf.MoveTowards(fishPosition, fishDestination, fishSpeed * Time.deltaTime);

        if (Mathf.Abs(fishPosition - fishDestination) < 10f)
        {
            fishDestination = Random.Range(bottomBoundary, topBoundary);
        }

        fishIcon.anchoredPosition = new Vector2(0, fishPosition);
    }

    void CatchAreaMovement()
    {
        if (Input.GetMouseButton(0))
        {
            catchAreaPosition += catchAreaSpeed * Time.deltaTime;
        }
        else
        {
            catchAreaPosition -= catchAreaSpeed * Time.deltaTime;
        }

        catchAreaPosition = Mathf.Clamp(catchAreaPosition, bottomBoundary, topBoundary);
        catchArea.anchoredPosition = new Vector2(0, catchAreaPosition);
    }

    void CheckProgress()
    {
        float distance = Mathf.Abs(fishPosition - catchAreaPosition);

        if (distance < catchArea.rect.height / 2f)
        {
            progressBar.value += fillSpeed * Time.deltaTime; 
        }
        else
        {
            progressBar.value -= loseSpeed * Time.deltaTime; 
        }

        if (progressBar.value >= progressBar.maxValue)
        {
            miniGamePanel.SetActive(false);

            PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddFish(currentFishToCatch);
            }
        }
    }
}