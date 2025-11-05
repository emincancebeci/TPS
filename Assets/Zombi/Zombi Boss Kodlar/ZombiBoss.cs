using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombiBoss : MonoBehaviour
{
    private float zombibossHP = 200;
    Animator bossAnim;
    bool bossOlu;
    public float kovalamamesafe;
    public float saldirmamesafesi;

    GameObject oyuncuajan;
    NavMeshAgent ZombiBossNavMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossAnim = this.GetComponent<Animator>();
        oyuncuajan = GameObject.Find("Ajan");
    }

    // Update is called once per frame
    void Update()
    {
        if (zombibossHP <= 0)
        {
            bossOlu=true;
        }
        if(bossOlu==true)    
        {
            bossAnim.SetBool("zombiebossoldu", true);
            StartCoroutine(Yokol());
            ZombiBossNavMesh=this.GetComponent<NavMeshAgent>();
        }
        else
        {
            //Ýleride hareket kod yazýlacak
            float mesafe = Vector3.Distance(this.transform.position, oyuncuajan.transform.position);
            if(mesafe <kovalamamesafe)
            {
                ZombiBossNavMesh.isStopped = false;
                ZombiBossNavMesh.SetDestination(oyuncuajan.transform.position);
                bossAnim.SetBool("yuruyor", true);
                this.transform.LookAt(oyuncuajan.transform.position);
            }
            else
            {
                ZombiBossNavMesh.isStopped=true;
                bossAnim.SetBool("yuruyor", false);
                bossAnim.SetBool("saldiriyor", false);
            }
            if(mesafe < saldirmamesafesi)
            {
                this.transform.LookAt(oyuncuajan.transform.position);
                ZombiBossNavMesh.isStopped = true;
                bossAnim.SetBool("yuruyor", false);
                bossAnim.SetBool("saldiriyor", true);
            }
        }
       
    }
    IEnumerator Yokol()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
    public void HasarAlBoss()
    {
        zombibossHP -= Random.Range(5, 10);
    }
}
