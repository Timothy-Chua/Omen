using System.Collections;
using TMPro;
using UnityEngine;

public class WalisManager : MonoBehaviour, IMinigame
{
    public static WalisManager instance;

    [Header("Game Settings")]
    [SerializeField] private GameObject[] interactableObjects;
    private float currentTime;
    [SerializeField] private float timeToComplete = 10f;
    [SerializeField] private int itemsToCollect = 5;
    [HideInInspector] public int itemsCollected;
    [HideInInspector] public bool isStart, isEnd;
    private bool isWin;

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private GameObject panelWalis;
    [SerializeField] private GameObject timeDisplay;
    [SerializeField] private TMP_Text txtTime, txtResult, txtInstructions;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public void Initialize()
    {
        timeDisplay.SetActive(false);
        txtInstructions.gameObject.SetActive(false);
        txtResult.gameObject.SetActive(false);

        panelWalis.SetActive(true);

        isStart = false;
        isEnd = false;
        isWin = false;

        itemsCollected = 0;
        currentTime = 360f - timeToComplete;
        txtTime.text = TimeToString(currentTime);
        foreach (GameObject obj in interactableObjects)
        {
            obj.SetActive(true);
        }

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
                if (itemsCollected < itemsToCollect)
                {
                    if (currentTime < 360)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else
                    {
                        isWin = false;
                        isEnd = true;
                    }
                }
                else
                {
                    isWin = true;
                    isEnd = true;
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
        timeDisplay.SetActive(true);
        bgSource.Play();
        isStart = true;
    }

    private IEnumerator EndCutscene()
    {
        isStart = false;

        yield return new WaitForSeconds(0.5f);

        txtResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        bgSource.Stop();
        panelWalis.gameObject.SetActive(false);
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
