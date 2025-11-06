using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        // UI 'ın click alması icin EventSystem  
        if (FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OyunuDurdur()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        OyunDurdu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OyundanCik()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // UI Button: Restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}
