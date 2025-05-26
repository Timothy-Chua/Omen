using System.Collections;
using TMPro;
using UnityEngine;

public class HairManager : MonoBehaviour, IMinigame
{
    public static HairManager instance;

    [Header("Game Settings")]
    [SerializeField] private int numberKnotsToComplete = 4;
    [HideInInspector] public int currentKnots;
    [SerializeField] private float timeToComplete = 7f;
    private float currentTime;
    [SerializeField] private GameObject[] hairObjects;
    [HideInInspector] public bool isStart, isEnd, isWin;

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private GameObject panelHair;
    [SerializeField] private GameObject timeDisplay;
    [SerializeField] private TMP_Text txtTime, txtResult, txtInstructions;

    void Awake()
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
        currentKnots = 0;
        foreach(GameObject obj in hairObjects) { obj.SetActive(false); }

        RandomKnots();

        txtTime.text = TimeToString(currentTime);
        panelHair.SetActive(true);
        txtInstructions.gameObject.SetActive(false);
        txtResult.gameObject.SetActive(false);
        timeDisplay.SetActive(false);

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
                if (currentKnots < numberKnotsToComplete)
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

    private void RandomKnots()
    {
        int knotsActivated = 0;
        while (knotsActivated < numberKnotsToComplete)
        {
            int knotToActivate = Random.Range(0, hairObjects.Length);
            if (!hairObjects[knotToActivate].activeInHierarchy)
            {
                hairObjects[knotToActivate].SetActive(true);
                knotsActivated++;
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
    }

    private IEnumerator EndCutscene()
    {
        isStart = false;

        yield return new WaitForSeconds(0.5f);

        txtResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        bgSource.Stop();
        panelHair.SetActive(false);
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
