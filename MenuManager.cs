using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private PanelStatus panelStatus;

    [Header("Panels")]
    [SerializeField] private GameObject panelMain;
    [SerializeField] private GameObject panelEduc, panelCredits;
    [SerializeField] private GameObject[] panelFolkInfo;
    private int currentFolklore;

    private enum PanelStatus
    {
        panelMain, panelEduc, panelCredits
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _BtnMain();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            switch (panelStatus)
            {
                case PanelStatus.panelEduc:
                    _BtnMain();
                    break;
                case PanelStatus.panelCredits:
                    _BtnMain();
                    break;
                case PanelStatus.panelMain:
                    break;
            }
        }
    }

    public void _BtnPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void _BtnMain()
    {
        panelMain.SetActive(true);
        panelEduc.SetActive(false);
        panelCredits.SetActive(false);
    }

    public void _BtnEduc()
    {
        panelMain.SetActive(false);
        panelEduc.SetActive(true);
        panelCredits.SetActive(false);

        for (int i = 0; i < panelFolkInfo.Length; i++)
        {
            if (i == currentFolklore)
            {
                panelFolkInfo[i].SetActive(true);
                continue;
            }
                
            panelFolkInfo[i].SetActive(false);
        }
    }

    public void _BtnCredit()
    {
        panelMain.SetActive(false);
        panelEduc.SetActive(true);
        panelCredits.SetActive(true);
    }

    public void _BtnNext()
    {
        panelFolkInfo[currentFolklore].SetActive(false);

        if (currentFolklore >= panelFolkInfo.Length - 1)
            currentFolklore = 0;
        else
            currentFolklore++;

        panelFolkInfo[currentFolklore].SetActive(true);
    }

    public void _BtnQuit()
    {
        Application.Quit();
    }
}
