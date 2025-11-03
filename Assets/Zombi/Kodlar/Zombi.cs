using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Zombi : MonoBehaviour
{
    public float ZombiHP = 100;
    Animator zombiAnim;
    bool zombiOlu;
    bool deathNotified;
    public float KovalamaMesafe;
    public float saldırmaMesafesi;
    NavMeshAgent zombiNavMash;

    GameObject hedefOyuncu;

    AudioSource sesKaynagi;
    public AudioClip saldirmaSesi;
    void Start()
    {
        zombiAnim = this.GetComponent<Animator>();
        hedefOyuncu = GameObject.Find("Ajan");
        zombiNavMash = this.GetComponent<NavMeshAgent>();
        sesKaynagi = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (ZombiHP <= 0)
        {
            zombiOlu = true;
            if (!deathNotified)
            {
                var spawner = FindObjectOfType<ZombieSpawner>();
                if (spawner != null)
                {
                    spawner.NotifyDeathStart(this.gameObject);
                }
                deathNotified = true;
            }
        }
        if (zombiOlu == true)
        {
            zombiAnim.SetBool("oldu", true);
            StartCoroutine(YokOl());
        }
        else
        {
            //ileride hareket kodunu buraya yazacağız
            float mesafe = Vector3.Distance(this.transform.position, hedefOyuncu.transform.position);
            if (mesafe < KovalamaMesafe)
            {
                zombiNavMash.isStopped = false;
                zombiNavMash.SetDestination(hedefOyuncu.transform.position);
                zombiAnim.SetBool("yuruyor", true);
                this.transform.LookAt(hedefOyuncu.transform.position);
                //yürüme animasyonu
            }
            else
            {
                zombiNavMash.isStopped = true;
                zombiAnim.SetBool("yuruyor", false);
                zombiAnim.SetBool("saldiriyor", false);
                //durma animasyonu
            }
            if (mesafe < saldırmaMesafesi)
            {
                this.transform.LookAt(hedefOyuncu.transform.position);
                zombiNavMash.isStopped = true;
                zombiAnim.SetBool("yuruyor", false);
                zombiAnim.SetBool("saldiriyor", true);
                //vurma animasyonu
            }

        }
    }
    public void HasarVerSes()
    {
        sesKaynagi.PlayOneShot(saldirmaSesi);
    }
    public void HasarVer()
    {
        hedefOyuncu.GetComponent<KarakrerKontrol>().HasarAl();
    }

    IEnumerator YokOl()

    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
    public void HasarAl()
    {
        ZombiHP -= Random.Range(15, 25);
    }
}

