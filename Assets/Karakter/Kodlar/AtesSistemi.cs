using Unity.Mathematics;
using UnityEngine;

public class AtesSistemi : MonoBehaviour
{



    public Camera kamera;
    public LayerMask zombiKatman;
    KarakrerKontrol hpKontrol;
    public ParticleSystem muzzleFlash;
    Animator anim;

    private float sarjor = 15;
    private float cephane = 150;
    private float sarjorKapasitesi = 15;

    AudioSource sesKaynagi;
    public AudioClip atesSes;
    public AudioClip reloadSes;


    void Start()
    {
        kamera = Camera.main;
        hpKontrol = this.gameObject.GetComponent<KarakrerKontrol>();
        anim = this.gameObject.GetComponent<Animator>();
        sesKaynagi = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (hpKontrol.YasiyorMu() == true)
        {

            if (Input.GetMouseButton(0))
            {

                if (sarjor > 0)
                {
                    anim.SetBool("atesEt", true);
                    // Upper-body firing flag to allow shooting while running/jumping
                    anim.SetBool("IsFiring", true);
                }
                if (sarjor < 0)
                {
                    anim.SetBool("atesEt", false);
                }
                if (sarjor <= 0 && cephane > 0)
                {
                    anim.SetBool("sarjorDegistirme", true);

                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                anim.SetBool("atesEt", false);
                anim.SetBool("IsFiring", false);
            }

        }
    }
    public void SarjorDegistirmeSes()
    {
        sesKaynagi.PlayOneShot(reloadSes);
        sesKaynagi.volume = 0.6f;
    }

    public void SarjorDegistirme()
    {
        sesKaynagi.volume = 1f;
        cephane -= sarjorKapasitesi - sarjor;
        sarjor = sarjorKapasitesi;
        anim.SetBool("sarjorDegistirme", false);
    }
    public void AtesEtme()
    {
        if (sarjor > 0)
        {
            sarjor--;
            muzzleFlash.Play();
            sesKaynagi.PlayOneShot(atesSes);

            // Crosshair'ın yeni pozisyonuna göre ışın yolla (Y değeri 0.7f = üst kısım)
            Ray ray = kamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombiKatman))
            {
                hit.collider.gameObject.GetComponent<Zombi>().HasarAl();
            }
        }


    }

    public float GetSarjor()
    {
        return sarjor;
    }
    public float GetCephane()
    {
        return cephane;
    }


    // Adds ammo to reserve; optionally triggers reload animation if magazine is empty
    public void AddAmmo(int amount, bool autoReload = true)
    {
        if (amount <= 0) return;
        cephane += amount;
        if (autoReload && sarjor <= 0 && cephane > 0)
        {
            anim.SetBool("sarjorDegistirme", true);
        }
    }


}

