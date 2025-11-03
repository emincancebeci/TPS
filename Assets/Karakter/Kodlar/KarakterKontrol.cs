using UnityEngine;

public class KarakrerKontrol : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    private float KarakterHiz;
    [SerializeField] private float sprintMultiplier = 1.5f;
    
    bool hayattaMi;
    private float saglik = 100;

    
    
    void Start()
    {
        anim = this.GetComponent<Animator>();
        hayattaMi = true;

    }

    void Update()
    {
        if (saglik <= 0)
        {
            hayattaMi = false;
            anim.SetBool("yasiyorMu", hayattaMi);
        }
        if (hayattaMi == true)
        {
            Hareket();
        }
    }

    public float GetSaglik()
    {
        return saglik;
    }
    public bool YasiyorMu()
    {
        return hayattaMi;
    }
    public void HasarAl()
    {
        saglik -= Random.Range(5, 10);
    }
    void Hareket()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");

        // Animator inputs
        anim.SetFloat("Horizontal", yatay);
        anim.SetFloat("Vertical", dikey);

        // Sprint (Left Shift) – sadece hız çarpanı
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = KarakterHiz * (isSprinting ? sprintMultiplier : 1f);

        // Yalnızca yatay düzlemde hareket
        this.gameObject.transform.Translate(yatay * speed * Time.deltaTime, 0, dikey * speed * Time.deltaTime);

        // İsteğe bağlı: hareket yönüne bakma
        Vector3 planar = new Vector3(yatay, 0f, dikey);
        if (planar.sqrMagnitude > 0.001f) transform.forward = planar.normalized;
    }



}

