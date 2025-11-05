using UnityEngine;
using UnityEngine.UI;

public class ArayuzKontrol : MonoBehaviour
{

    public Text mermiText;
    public Text saglikText;
    public GameObject PauseMenu;

    bool OyunDurdu;
    GameObject oyuncu;

    void Start()
    {
        oyuncu = GameObject.Find("Ajan");
    }

    // Update is called once per frame
    void Update()
    {
        mermiText.text = oyuncu.GetComponent<AtesSistemi>().GetSarjor().ToString() + "/" + oyuncu.GetComponent<AtesSistemi>().GetCephane().ToString();

        saglikText.text = "Hp:" + oyuncu.GetComponent<KarakrerKontrol>().GetSaglik();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OyunDurdu == true)
            {
                OyunuDevamEttir();
            }
            else if (OyunDurdu == false)
            {
                OyunuDurdur();
            }

        }
    }
    public void OyunuDevamEttir()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        OyunDurdu = false;
    }
    public void OyunuDurdur()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        OyunDurdu = true;
    }

    public void OyundanCik()
    {
        Application.Quit();
    }
}
