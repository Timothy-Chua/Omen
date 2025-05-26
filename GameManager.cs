using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Stats")]
    private int lives;                                  // The current number of lives the player has
    [SerializeField] private int livesInital = 4;       // The initial number of lives given to the player
    private int levelAt;                                 // Current level of the player
    [SerializeField] private GameObject[] minigames;    // Holds the parents of each minigame
    [SerializeField] private GameObject bossLevel;      // Holds the boss level parent
    private int gameToLoad, previousLevelIndex;                             // Indicates the next minigame to load

    [Header("UI & Cutscene")]
    [SerializeField] private Camera camTransition;
    [SerializeField] private VideoPlayer transitionPlayer;
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private VideoClip clipWin, clipLose;
    [SerializeField] private AudioClip sfxWin, sfxLose;
    [SerializeField] private GameObject panelMenuCopy, panelTransition, gameOverDisplay, levelDisplay;
    [SerializeField] private TMP_Text txtLevelAt;
    [SerializeField] private GameObject[] lifeIndicators;
    public string winText = "Complete!", loseText = "Fail.";


    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelAt = 1;
        lives = livesInital;

        gameOverDisplay.SetActive(false);
        txtLevelAt.text = levelAt.ToString();
        StartCoroutine(StartCutscene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelEnd(bool winCondition)
    {
        if (!winCondition)
        {
            if (lives > 1)
            {
                lives -= 1;
                lifeIndicators[lives].SetActive(false);
            }
            else
            {
                lives -= 1;
                lifeIndicators[lives].SetActive(false);
                StartCoroutine(GameOver());
                return;
            }
        }

        transitionPlayer.clip = winCondition ? clipWin : clipLose;
        sfxPlayer.clip = winCondition ? sfxWin : sfxLose;

        levelAt++;
        txtLevelAt.text = levelAt.ToString();
        StartCoroutine(TransitionCutscene());
    }

    private IEnumerator GameOver()
    {
        panelTransition.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        DeactivatePreviousLevel();

        yield return new WaitForSeconds(1f);

        transitionPlayer.Play();
        sfxPlayer.PlayOneShot(sfxPlayer.clip, sfxPlayer.volume);

        levelDisplay.SetActive(false);
        gameOverDisplay.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        if (levelAt % 5 == 0)
        {
            bossLevel.SetActive(true);
            bossLevel.GetComponent<IMinigame>().Initialize();
        }
        else
        {
            while (true)
            {
                gameToLoad = Random.Range(0, minigames.Length);
                if (previousLevelIndex != gameToLoad)
                {
                    minigames[gameToLoad].SetActive(true);
                    minigames[gameToLoad].GetComponent<IMinigame>().Initialize();
                    previousLevelIndex = gameToLoad;
                    break;
                }
            }
        }

        camTransition.gameObject.SetActive(false);
    }

    private void DeactivatePreviousLevel()
    {
        if (levelAt % 6 == 0)
            bossLevel.SetActive(false);
        else
            minigames[gameToLoad].SetActive(false);

        camTransition.gameObject.SetActive(true);
    }

    private IEnumerator StartCutscene()
    {
        panelTransition.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        panelMenuCopy.SetActive(false);

        yield return new WaitForSeconds(4.5f);

        LoadNextLevel();

        yield return new WaitForSeconds(2f);

        panelTransition.SetActive(false);
    }

    private IEnumerator TransitionCutscene()
    {
        panelTransition.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        DeactivatePreviousLevel();

        yield return new WaitForSeconds(1f);

        transitionPlayer.Play();
        sfxPlayer.PlayOneShot(sfxPlayer.clip, sfxPlayer.volume);

        yield return new WaitForSeconds(3.5f);

        LoadNextLevel();

        yield return new WaitForSeconds(2f);

        panelTransition.SetActive(false);
    }
}
