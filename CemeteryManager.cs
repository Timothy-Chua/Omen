using System.Collections;
using TMPro;
using UnityEngine;

public class CemeteryManager : MonoBehaviour, IMinigame
{
    public static CemeteryManager instance;

    [Header("Game Settings")]
    [SerializeField] private Transform startBabyPos;
    [SerializeField] private GameObject babyObj;
    [SerializeField] private float timeToComplete = 10f;
    private float currentTime;
    [HideInInspector] public bool isStart, isEnd, isWin;

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private GameObject panelCemetery, timeDisplay;
    [SerializeField] private TMP_Text txtTime, txtResult, txtInstructions;

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

        currentTime = timeToComplete;
        babyObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        babyObj.transform.position = startBabyPos.position;

        txtTime.text = TimeToString(currentTime);

        panelCemetery.SetActive(true);
        timeDisplay.SetActive(false);
        txtResult.gameObject.SetActive(false);
        txtInstructions.gameObject.SetActive(false);

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
                if (currentTime <= 0)
                {
                    isWin = false;
                    isEnd = true;
                }
                else
                {
                    currentTime -= Time.deltaTime;
                }
            }
            else
            {
                txtResult.text = isWin ? GameManager.instance.winText : GameManager.instance.loseText;
                StartCoroutine(EndCutscene());
            }
        }
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(2.5f);

        txtInstructions.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        txtInstructions.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        bgSource.Play();
        timeDisplay.SetActive(true);
        isStart = true;
        babyObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private IEnumerator EndCutscene()
    {
        isStart = false;
        babyObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(0.5f);

        txtResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        bgSource.Stop();
        panelCemetery.SetActive(false);
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
