using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PunsoManager : MonoBehaviour, IMinigame
{
    [Header("Game Settings")]
    [SerializeField] private float timeToComplete = 7f;
    private float currentTime;
    private bool isStart, isEnd, isWin;
    [SerializeField] private string winText = "Complete!", loseText = "Fail.";

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private GameObject panelPunso;
    [SerializeField] private GameObject timeDisplay, btnGreet;
    [SerializeField] private TMP_InputField inputResponse;
    [SerializeField] private TMP_Text txtResult, txtInstructions, txtTime;

    public void Initialize() 
    {
        currentTime = timeToComplete;

        isStart = false;
        isEnd = false;
        isWin = false;

        txtTime.text = TimeToString(currentTime);
        panelPunso.SetActive(true);
        timeDisplay.SetActive(false);
        inputResponse.text = null;
        inputResponse.gameObject.SetActive(false);
        btnGreet.SetActive(false);
        txtInstructions.gameObject.SetActive(false);
        txtResult.gameObject.SetActive(false);

        StartCoroutine(StartCutscene());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            txtTime.text = TimeToString(currentTime);

            if (!isEnd)
            {
                if (Input.GetButtonDown("Submit"))
                    _BtnGreet();

                if (currentTime <= 0)
                {
                    isWin = false;
                    isEnd = true;
                }
                else
                    currentTime -= Time.deltaTime;
            }
            else
            {
                txtResult.text = isWin ? winText : loseText;
                StartCoroutine(EndCutscene());
            }
        }
    }

    public void _BtnGreet()
    {
        string response = inputResponse.text.ToLower();
        response = response.Replace(" ", "");
        isWin = (response == "tabi-tabipo") ? true : false;
        isEnd = true;
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(2.5f);

        txtInstructions.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        txtInstructions.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        inputResponse.gameObject.SetActive(true);
        btnGreet.SetActive(true);
        timeDisplay.SetActive(true);
        bgSource.Play();
        isStart = true;
    }

    private IEnumerator EndCutscene()
    {
        isStart = false;

        inputResponse.gameObject.SetActive(false);
        btnGreet.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        txtResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        bgSource.Stop();
        panelPunso.SetActive(false);
        GameManager.instance.LevelEnd(isWin);
    }

    private string TimeToString(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;

        string newTime = string.Format("{00:00}:{01:00}", minutes, seconds);
        return newTime;
    }
}
