using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButterflyManager : MonoBehaviour, IMinigame
{
    public static ButterflyManager instance;

    [Header("Game Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] butterflies;
    [SerializeField] private float timeToComplete = 10f, timeToSpawn = 0.5f;
    public float butterflySpeed = 1.25f;
    private float currentTime, currentSpawnTime;
    [HideInInspector] public bool isStart, isEnd, isWin;

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private GameObject panelButterfly;
    [SerializeField] private GameObject timeDisplay;
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
        
        currentSpawnTime = timeToSpawn;
        currentTime = timeToComplete;

        foreach (GameObject obj in butterflies) 
        {
            Animator animator = obj.GetComponent<Animator>();
            animator.speed = 1f;
            if (obj.activeInHierarchy) obj.SetActive(false); 
        }

        txtTime.text = TimeToString(currentTime);
        panelButterfly.SetActive(true);
        timeDisplay.SetActive(false);
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
                if (currentSpawnTime <= 0)
                {
                    SpawnButterfly();
                    currentSpawnTime = timeToSpawn;
                }
                else
                    currentSpawnTime -= Time.deltaTime;

                if (currentTime <= 0)
                {
                    isEnd = true;
                    isWin = false;
                }
                else
                    currentTime -= Time.deltaTime;
            }
            else
            {
                foreach (GameObject obj in butterflies)
                {
                    Animator animator = obj.GetComponent<Animator>();
                    animator.speed = 0f;
                }
                txtResult.text = isWin ? GameManager.instance.winText : GameManager.instance.loseText;
                StartCoroutine(EndCutscene());
            }
        }
    }

    private void SpawnButterfly()
    {
        GameObject butterflyToSpawn = null;
        foreach (GameObject obj in butterflies)
        {
            int random = Random.Range(0, butterflies.Length);
            if (!butterflies[random].activeInHierarchy)
            {
                butterflyToSpawn = butterflies[random];

                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                butterflyToSpawn.transform.position = spawnPoint.position;
                butterflyToSpawn.SetActive(true);

                butterflyToSpawn.GetComponent<ButterflyInteract>().Initiate();

                break;
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
        panelButterfly.SetActive(false);
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
