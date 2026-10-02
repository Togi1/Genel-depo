using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    public float takipHizi = 5f;       // Kameranýn darta yetiþme hýzý
    public float geriDonusHizi = 2f;   // Eski yerine dönme hýzý

    private Transform hedefDart;       // Þu an takip edilen dart
    private Vector3 baslangicKonumu;   // Kameranýn ilk durduðu yer
    private Quaternion baslangicRotasyonu; // Kameranýn ilk baktýðý yön
    private Vector3 farkMesafesi;      // Kamera ile dart arasýndaki mesafe farký (Offset)

    void Start()
    {
        // Oyun baþlarken kameranýn nerede durduðunu hafýzaya atýyoruz
        baslangicKonumu = transform.position;
        baslangicRotasyonu = transform.rotation;
    }

    void LateUpdate()
    {
        // Senaryo 1: Ortada bir hedef (Fýrlatýlan Dart) varsa onu takip et
        if (hedefDart != null)
        {
            // Dartýn pozisyonuna, aradaki farký (offset) ekleyerek git
            Vector3 istenenKonum = hedefDart.position + farkMesafesi;

            // Lerp komutu ile yumuþakça (sinematik) kaydýrýyoruz
            transform.position = Vector3.Lerp(transform.position, istenenKonum, takipHizi * Time.deltaTime);
        }
        // Senaryo 2: Hedef yoksa (iþ bittiyse) eski yerine usulca dön
        else
        {
            transform.position = Vector3.Lerp(transform.position, baslangicKonumu, geriDonusHizi * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, baslangicRotasyonu, geriDonusHizi * Time.deltaTime);
        }
    }

    // Bu fonksiyonu DartFirlatma kodundan çaðýracaðýz
    public void TakibeBasla(Transform yeniHedef)
    {
        hedefDart = yeniHedef;
        // Fýrlatma anýnda kamera ile dart arasýndaki mesafeyi koru
        farkMesafesi = transform.position - yeniHedef.position;
    }

    // Ýþ bitince bu fonksiyonu çaðýracaðýz
    public void TakibiBirak()
    {
        hedefDart = null;
    }
}