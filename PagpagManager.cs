using System.Collections;
using TMPro;
using UnityEngine;

public class PagpagManager : MonoBehaviour, IMinigame
{
    public static PagpagManager instance;

    [Header("Game Settings")]
    [SerializeField] public Transform player;
    [SerializeField] private Transform startPos, targetPos;
    [SerializeField] private float playerSpeed = 1f;
    [SerializeField] private float timeToComplete = 15f;
    private float currentTime;
    [HideInInspector] public bool isStart, isEnd, isWin;

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private GameObject panelPagpag;
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

        player.GetComponent<Animator>().speed = 0f;

        currentTime = timeToComplete;
        player.position = new Vector3(startPos.position.x, startPos.position.y, player.position.z);
        player.gameObject.SetActive(true);

        txtTime.text = TimeToString(currentTime);
        panelPagpag.SetActive(true);
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
                player.position = Vector3.Lerp(player.position, targetPos.position, playerSpeed * Time.deltaTime);

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

        player.GetComponent<Animator>().speed = 1f;
        isStart = true;
    }

    private IEnumerator EndCutscene()
    {
        isStart = false;

        yield return new WaitForSeconds(0.5f);

        txtResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        bgSource.Stop();
        panelPagpag.SetActive(false);
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
