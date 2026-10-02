using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OyunYoneticisi : MonoBehaviour
{
    [Header("Puan Ayarlarý")]
    public int toplamPuan = 0;
    public int enYuksekPuan = 0;
    public int enYuksekPuanSureli = 0; // Süreli modun ayrý rekoru
    public TextMeshProUGUI puanText;
    public TextMeshProUGUI enYuksekPuanText;

    [Header("Oyun Döngüsü Ayarlarý")]
    public int toplamDartHakki = 3;
    private int kalanDart;
    public TextMeshProUGUI kalanDartText;
    public GameObject gameOverPaneli;
    public TextMeshProUGUI sonucText;

    [Header("Oyun Modu Ayarlarý")]
    public GameObject sifirlaButonu;
    public TextMeshProUGUI sureText; // YENÝ: Süre yazýsý
    private bool sinirsizModAcik = false;
    private bool sureliModAcik = false;
    private float sure = 60f; // 60 saniyeden geri sayacak
    private bool oyunDevamEdiyor = true;

    [Header("Duraklatma Menüsü")]
    public GameObject duraklatmaPaneli;

    [Header("Dart Üretimi")]
    public GameObject dartPrefab;
    public Transform baslangicNoktasi;

    void Start()
    {
        Time.timeScale = 1f;

        enYuksekPuan = PlayerPrefs.GetInt("RekorPuan", 0);
        enYuksekPuanSureli = PlayerPrefs.GetInt("RekorPuanSureli", 0);

        int mod = PlayerPrefs.GetInt("OyunModu", 0);

        if (mod == 1) // Sýnýrsýz Mod
        {
            sinirsizModAcik = true;
            if (kalanDartText != null) kalanDartText.gameObject.SetActive(false);
            if (sifirlaButonu != null) sifirlaButonu.SetActive(true);
            if (sureText != null) sureText.gameObject.SetActive(false);
        }
        else if (mod == 2) // Süreli Mod
        {
            sureliModAcik = true;
            if (kalanDartText != null) kalanDartText.gameObject.SetActive(false);
            if (sifirlaButonu != null) sifirlaButonu.SetActive(false);
            if (sureText != null) sureText.gameObject.SetActive(true);
        }
        else // Normal Mod
        {
            kalanDart = toplamDartHakki;
            if (kalanDartText != null) kalanDartText.gameObject.SetActive(true);
            if (sifirlaButonu != null) sifirlaButonu.SetActive(false);
            if (sureText != null) sureText.gameObject.SetActive(false);
        }

        if (gameOverPaneli != null) gameOverPaneli.SetActive(false);
        if (duraklatmaPaneli != null) duraklatmaPaneli.SetActive(false);

        EkraniGuncelle();
        YeniDartUret();
    }

    // YENÝ: Geri sayým motoru
    void Update()
    {
        if (sureliModAcik && oyunDevamEdiyor)
        {
            sure -= Time.deltaTime; // Saniyeleri düþür

            if (sure <= 0)
            {
                sure = 0;
                oyunDevamEdiyor = false;
                OyunBitti(); // Süre bitince paneli aç
            }
            EkraniGuncelle();
        }
    }

    public void PuanEkle(int kazanilanPuan)
    {
        if (sureliModAcik && !oyunDevamEdiyor) return; // Süre bittiyse geç gelen oklarýn puanýný sayma

        toplamPuan += kazanilanPuan;

        if (sureliModAcik)
        {
            if (toplamPuan > enYuksekPuanSureli)
            {
                enYuksekPuanSureli = toplamPuan;
                PlayerPrefs.SetInt("RekorPuanSureli", enYuksekPuanSureli);
                PlayerPrefs.Save();
            }
        }
        else
        {
            if (toplamPuan > enYuksekPuan)
            {
                enYuksekPuan = toplamPuan;
                PlayerPrefs.SetInt("RekorPuan", enYuksekPuan);
                PlayerPrefs.Save();
            }
        }

        EkraniGuncelle();
    }

    void EkraniGuncelle()
    {
        if (puanText != null) puanText.text = "Puan: " + toplamPuan.ToString();

        if (!sinirsizModAcik && !sureliModAcik && kalanDartText != null)
            kalanDartText.text = "Kalan Dart: " + kalanDart.ToString();

        // Süreyi tam sayý olarak göster
        if (sureliModAcik && sureText != null)
            sureText.text = "Süre: " + Mathf.CeilToInt(sure).ToString();

        if (enYuksekPuanText != null)
        {
            if (sureliModAcik) enYuksekPuanText.text = "Rekor (60s): " + enYuksekPuanSureli.ToString();
            else enYuksekPuanText.text = "Rekor: " + enYuksekPuan.ToString();
        }
    }

    public void YeniDartUret()
    {
        if (sureliModAcik)
        {
            // Süre bitmediði müddetçe sürekli dart ver
            if (oyunDevamEdiyor) Instantiate(dartPrefab, baslangicNoktasi.position, baslangicNoktasi.rotation);
        }
        else if (sinirsizModAcik)
        {
            Instantiate(dartPrefab, baslangicNoktasi.position, baslangicNoktasi.rotation);
        }
        else
        {
            if (kalanDart > 0)
            {
                kalanDart--;
                EkraniGuncelle();
                Instantiate(dartPrefab, baslangicNoktasi.position, baslangicNoktasi.rotation);
            }
            else
            {
                OyunBitti();
            }
        }
    }

    void OyunBitti()
    {
        oyunDevamEdiyor = false;
        if (gameOverPaneli != null)
        {
            gameOverPaneli.SetActive(true);
            int gecerliRekor = sureliModAcik ? enYuksekPuanSureli : enYuksekPuan;
            if (sonucText != null)
                sonucText.text = "OYUN BÝTTÝ!\nToplam Puan: " + toplamPuan.ToString() + "\nEn Yüksek Puan: " + gecerliRekor.ToString();
        }
    }

    public void TekrarOyna() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void MenuyeDon() { Time.timeScale = 1f; SceneManager.LoadScene("AnaMenu"); }
    public void TahtayiSifirla() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void OyunuDurdur() { if (duraklatmaPaneli != null) duraklatmaPaneli.SetActive(true); Time.timeScale = 0f; }
    public void OyunaDevamEt() { if (duraklatmaPaneli != null) duraklatmaPaneli.SetActive(false); Time.timeScale = 1f; }
}