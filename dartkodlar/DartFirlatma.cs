using UnityEngine;

public class DartFirlatma : MonoBehaviour
{
    Vector2 baslangicNoktasi;
    Vector2 bitisNoktasi;

    [Header("Fırlatma Ayarları")]
    public float firlatmaGucu = 0.05f;
    public float ileriGuc = 20f;
    public float yereDusmeSiniri = -5f;

    [Header("Şans ve Fizik Ayarları")]
    public int sekmeSansi = 5;
    public float sekmeGucu = 5f;
    public float dususHiziLimiti = -2.5f;

    [Header("Görsel Efektler")]
    public GameObject vurusEfektiPrefab;
    public GameObject yuzenYaziPrefab;

    [Header("Ses Efektleri")]
    public AudioClip firlatmaSesi;
    public AudioClip saplanmaSesi;
    public AudioClip sekmeSesi;
    private AudioSource sesOynatici;

    Rigidbody rb;
    bool firlatildiMi = false;
    bool islemTamamlandi = false;

    KameraTakip kameraScripti;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // Dartın üzerindeki hoparlörü bul
        sesOynatici = GetComponent<AudioSource>();

        if (Camera.main != null)
        {
            kameraScripti = Camera.main.GetComponent<KameraTakip>();
        }
    }

    void Update()
    {
        if (firlatildiMi && !islemTamamlandi && transform.position.y < yereDusmeSiniri)
        {
            KaravanaIleBitir();
        }

        if (firlatildiMi && !islemTamamlandi && rb.linearVelocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity) * Quaternion.Euler(90, 0, 0);
        }

        if (islemTamamlandi || firlatildiMi) return;

        if (Input.GetMouseButtonDown(0))
        {
            baslangicNoktasi = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            bitisNoktasi = Input.mousePosition;
            Firlat();
        }
    }

    void Firlat()
    {
        firlatildiMi = true;
        rb.isKinematic = false;
        Vector2 kaydirmaYonu = bitisNoktasi - baslangicNoktasi;
        Vector3 firlatmaVektoru = new Vector3(kaydirmaYonu.x * firlatmaGucu, kaydirmaYonu.y * firlatmaGucu, ileriGuc);
        rb.AddForce(firlatmaVektoru, ForceMode.Impulse);

        // Fırlatma sesini çal
        if (firlatmaSesi != null && sesOynatici != null)
        {
            sesOynatici.PlayOneShot(firlatmaSesi);
        }

        if (kameraScripti != null)
        {
            kameraScripti.TakibeBasla(this.transform);
        }
    }

    void KaravanaIleBitir()
    {
        islemTamamlandi = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        KamerayiSifirla();
        Invoke("YeniDartCagir", 1f);
    }

    void YeniDartCagir()
    {
        Object.FindFirstObjectByType<OyunYoneticisi>().YeniDartUret();
    }

    void KamerayiSifirla()
    {
        if (kameraScripti != null)
        {
            kameraScripti.TakibiBirak();
        }
    }

    void OnCollisionEnter(Collision temas)
    {
        if (islemTamamlandi) return;

        if (temas.gameObject.CompareTag("Tahta"))
        {
            islemTamamlandi = true;
            bool asagiDusuyor = rb.linearVelocity.y < dususHiziLimiti;
            int zar = Random.Range(0, 100);

            if (asagiDusuyor || zar < sekmeSansi)
            {
                // Sekme sesini çal
                if (sekmeSesi != null && sesOynatici != null) sesOynatici.PlayOneShot(sekmeSesi);

                rb.AddForce(Vector3.back * 2f + Vector3.down * 2f, ForceMode.Impulse);
                Invoke("KamerayiSifirla", 1f);
                Invoke("YeniDartCagir", 1.5f);
            }
            else
            {
                // Saplanma (Tahta) sesini çal
                if (saplanmaSesi != null && sesOynatici != null) sesOynatici.PlayOneShot(saplanmaSesi);

                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                // --- Görsel Efektler ---
                if (vurusEfektiPrefab != null) Instantiate(vurusEfektiPrefab, transform.position, Quaternion.identity);

                PuanBolgesi vurulanBolge = temas.gameObject.GetComponent<PuanBolgesi>();
                if (vurulanBolge != null)
                {
                    Object.FindFirstObjectByType<OyunYoneticisi>().PuanEkle(vurulanBolge.puan);

                    // Yüzen Puan Yazısı
                    if (yuzenYaziPrefab != null)
                    {
                        GameObject yaziObje = Instantiate(yuzenYaziPrefab, transform.position + (Vector3.up * 0.2f) + (Vector3.back * 0.2f), Quaternion.identity);
                        yaziObje.transform.LookAt(Camera.main.transform);
                        yaziObje.transform.Rotate(0, 180, 0);

                        YuzenYazi yuzenKod = yaziObje.GetComponent<YuzenYazi>();
                        if (yuzenKod != null) yuzenKod.PuanAyarla(vurulanBolge.puan);
                    }
                }

                Invoke("KamerayiSifirla", 1.5f);
                Invoke("YeniDartCagir", 2f);
            }
        }
    }
}