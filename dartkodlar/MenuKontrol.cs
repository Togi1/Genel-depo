using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuKontrol : MonoBehaviour
{
    public GameObject ayarlarPaneli;

    public void OyunaBasla()
    {
        PlayerPrefs.SetInt("OyunModu", 0); // Normal
        SceneManager.LoadScene("DartOyunu");
    }

    public void SinirsizModBasla()
    {
        PlayerPrefs.SetInt("OyunModu", 1); // Sýnýrsýz
        SceneManager.LoadScene("DartOyunu");
    }

    // YENÝ: 60 Saniye Modu
    public void SureliModBasla()
    {
        PlayerPrefs.SetInt("OyunModu", 2); // 2 = Süreli Mod
        SceneManager.LoadScene("DartOyunu");
    }

    public void AyarlariAc() { if (ayarlarPaneli != null) ayarlarPaneli.SetActive(true); }
    public void AyarlariKapat() { if (ayarlarPaneli != null) ayarlarPaneli.SetActive(false); }
    public void OyundanCik() { Application.Quit(); }
}