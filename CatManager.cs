using System.Collections;
using TMPro;
using UnityEngine;

public class CatManager : MonoBehaviour, IMinigame
{
    public static CatManager instance;

    [Header("Game Settings")]
    [HideInInspector] public bool isCatVisible;
    [SerializeField] private float timeToComplete = 15f, timeCatUnseen = 3f;
    private float currentTime, currentTimeCatUnseen;
    [HideInInspector] public bool isStart, isEnd, isWin;
    [SerializeField] private Transform startPosCam, startPosCat;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject catObj;
    /*private SkinnedMeshRenderer catRenderer;*/

    [Header("UI")]
    [SerializeField] private AudioSource bgSource;
    [SerializeField] private GameObject panelCat, timeDisplay, controlDisplayL, controlDisplayR;
    [SerializeField] private TMP_Text txtTime, txtTimeCat, txtInstructions, txtResult;

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
        currentTimeCatUnseen = 0f;

        cam.transform.position = startPosCam.position;
        catObj.transform.position = startPosCat.position;

        /*catRenderer = catObj.GetComponentInChildren<SkinnedMeshRenderer>();*/
        catObj.GetComponentInChildren<Animator>().speed = 0f;

        txtTimeCat.text = TimeToString(currentTimeCatUnseen);
        txtTime.text = TimeToString(currentTime);

        panelCat.SetActive(true);
        timeDisplay.SetActive(false);
        txtTimeCat.gameObject.SetActive(false);
        txtInstructions.gameObject.SetActive(false);
        txtResult.gameObject.SetActive(false);
        controlDisplayL.SetActive(false);
        controlDisplayR.SetActive(false);

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
            if (!isEnd)
            {
                if (!isCatVisible)
                {
                    if (!txtTimeCat.gameObject.activeInHierarchy)
                        txtTimeCat.gameObject.SetActive(true);

                    txtTimeCat.text = currentTimeCatUnseen.ToString("0.00");

                    currentTimeCatUnseen += Time.deltaTime;
                    if (currentTimeCatUnseen >= timeCatUnseen)
                    {
                        isWin = true;
                        isEnd = true;
                    }
                }
                else
                {
                    if (txtTimeCat.gameObject.activeInHierarchy)
                        txtTimeCat.gameObject.SetActive(false);

                    currentTimeCatUnseen = 0f;
                }

                txtTime.text = TimeToString(currentTime);

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
        catObj.GetComponentInChildren<Animator>().speed = 1f;
        controlDisplayL.SetActive(true);
        controlDisplayR.SetActive(true);

        isStart = true;
    }

    private IEnumerator EndCutscene()
    {
        isStart = false;

        controlDisplayL.SetActive(false);
        controlDisplayR.SetActive(false);
        catObj.GetComponentInChildren<Animator>().speed = 0f;

        yield return new WaitForSeconds(0.5f);

        txtTimeCat.gameObject.SetActive(false);
        txtResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        bgSource.Stop();
        panelCat.SetActive(false);
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
