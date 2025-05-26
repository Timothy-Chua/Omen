using System.Collections;
using TMPro;
using UnityEngine;

public class GenderManager : MonoBehaviour, IMinigame
{
    public static GenderManager instance;

    [Header("Game Values")]
    private int randomGender;
    private GameObject chosenObject;
    [SerializeField] private GameObject objMale, objFemale;
    [SerializeField] private float timeToComplete = 5f;
    [SerializeField] private Transform objSpawnPos;
    private float currentTime;
    private bool isStart, isEnd, isWin;

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private Camera camFloor;
    [SerializeField] private GameObject panelGender, timeDisplay;
    [SerializeField] private TMP_Text txtTime, txtResult, txtInstructions;
    [SerializeField] private GameObject[] responseBtns;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public void Initialize()
    {
        isStart = false;
        isEnd = false;
        isWin = false;

        txtInstructions.gameObject.SetActive(false);
        txtResult.gameObject.SetActive(false);
        camFloor.gameObject.SetActive(false);
        if (chosenObject != null) { chosenObject.SetActive(false); }
        foreach (GameObject obj in responseBtns) { obj.SetActive(false); }
        timeDisplay.SetActive(false);

        panelGender.SetActive(true);
        randomGender = Random.Range(0, 11);
        currentTime = timeToComplete;
        txtTime.text = TimeToString(currentTime);

        StartCoroutine(StartCutscene());
    }

    private void Start()
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
                txtResult.text = isWin ? GameManager.instance.winText : GameManager.instance.loseText;
                StartCoroutine(EndCutscene());
            }
        }
    }

    public void _BtnMale()
    {
        isWin = (randomGender <= 6) ? true : false;
        isEnd = true;
    }

    public void _BtnFemale()
    {
        isWin = (randomGender > 6) ? true : false;
        isEnd = true;
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(2.5f);

        txtInstructions.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        txtInstructions.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        camFloor.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        chosenObject = (randomGender <= 6) ? objMale : objFemale;

        chosenObject.transform.position = objSpawnPos.position;
        chosenObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        timeDisplay.SetActive(true);
        foreach (GameObject obj in responseBtns)
            obj.SetActive(true);

        bgSource.Play();
        isStart = true;
    }

    private IEnumerator EndCutscene()
    {
        isStart = false;

        foreach (GameObject obj in responseBtns)
            obj.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        txtResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        bgSource.Stop();
        panelGender.SetActive(false);
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
