using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // UI Panels
    public GameObject mainMenu;
    public GameObject inGameMenu;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;

    // In game lobby
    public GameObject inGameLobby;

    public GameObject player;

    // Difficulty settings
    public SO_LevelDifficulty easyDifficulty;
    public SO_LevelDifficulty normalDifficulty;
    public SO_LevelDifficulty hardDifficulty;
    public SO_LevelDifficulty lunaticDifficulty;

    public GameObject tutorialStage;

    public bool hasStartedTutorial = false;
    public float gameStartTime = 0f;
    public float timeAlive = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        inGameMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        hasStartedTutorial = PlayerPrefs.GetInt("HasStartedTutorial", 0) == 1 ? true : false;
    }

    // On Start button clicked in main menu
    public void OnStartButtonClicked()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            AudioManager.Instance.PlayClickSfx();
            UIController.Instance.FadeOut();
        }).
        AppendInterval(1.5f).
        AppendCallback(() =>
        {
            mainMenu.SetActive(false);
            inGameMenu.SetActive(true);
            inGameMenu.transform.Find("Level Select").gameObject.SetActive(false);
            inGameLobby.SetActive(true);
            player.SetActive(true);
            player.transform.position = new Vector2(0, 0);
            UIController.Instance.FadeIn();
            InventoryManager.Instance.LoadInventory();
            player.GetComponent<PlayerInputController>().LoadWeapon();
        }).
        AppendInterval(1.3f).
        AppendCallback(() =>
        {
            UIController.Instance.ShowCurrentLevel("G");
        });
    }

    public void OnQuitButtonSelected()
    {
        AudioManager.Instance.PlayClickSfx();
        Application.Quit();
    }

    public void OnResetInventorySelected()
    {
        AudioManager.Instance.PlayClickSfx();
        foreach(GameObject weapon in player.GetComponent<PlayerInputController>().weapons)
        {
            foreach (ModularPartSlot part in weapon.GetComponent<WeaponBase>().slots)
            {
                if (part.part != null)
                {
                    weapon.GetComponent<WeaponBase>().UnequipPart(part.part.partType);
                }
            }
        }
        InventoryManager.Instance.ResetInventory();
    }

    public void OnResetTutorialSelected()
    {
        AudioManager.Instance.PlayClickSfx();
        PlayerPrefs.SetInt("HasStartedTutorial", 0);
        hasStartedTutorial = false;
    }

    public void DestroyLobby(float delay = 0f)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            inGameLobby.SetActive(false);
        });
    }

    public void OnDifficultySelected(int index)
    {
        AudioManager.Instance.PlayClickSfx();
        switch (index)
        {
            case 0:
                LevelManager.instance.currentDifficulty = Difficulty.Easy;
                LevelManager.instance.SetDifficulty(easyDifficulty);
                break;
            case 1:
                LevelManager.instance.currentDifficulty = Difficulty.Normal;
                LevelManager.instance.SetDifficulty(normalDifficulty);
                break;
            case 2:
                LevelManager.instance.currentDifficulty = Difficulty.Hard;
                LevelManager.instance.SetDifficulty(hardDifficulty);
                break;
            case 3:
                LevelManager.instance.currentDifficulty = Difficulty.Lunatic;
                LevelManager.instance.SetDifficulty(lunaticDifficulty);
                break;
            default:
                Debug.LogError("Invalid difficulty index selected.");
                return;
        }

        DestroyLobby(1.5f);
        LevelManager.instance.StartNewLevel();
        gameStartTime = Time.time;
    }

    public void OnTutorialSelected()
    {
        DestroyLobby(1.5f);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            AudioManager.Instance.PlayClickSfx();
            UIController.Instance.FadeOut();
        }).
        AppendInterval(1.5f).
        AppendCallback(() =>
        {
            player.transform.position = new Vector2(0, 0);
            GameObject tutorial = Instantiate(tutorialStage, LevelManager.instance.transform);
            UIController.Instance.FadeIn();
        }).
        AppendInterval(1.3f).
        AppendCallback(() =>
        {
            UIController.Instance.ShowCurrentLevel("LG");
            hasStartedTutorial = true;
            PlayerPrefs.SetInt("HasStartedTutorial", 1);
        });
    }

    public void OnGameOver()
    {
        timeAlive = Time.time - gameStartTime;
        gameOverMenu.SetActive(true);
    }

    public void OnRestartButtonClicked()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            AudioManager.Instance.PlayClickSfx();
            UIController.Instance.FadeOut();
        }).
        AppendInterval(1.5f).
        AppendCallback(() =>
        {
            gameOverMenu.SetActive(false);
            inGameLobby.SetActive(true);
            player.transform.position = new Vector2(0, 0);
            UIController.Instance.FadeIn();
            LevelManager.instance.ResetLevel();
            player.GetComponent<Character_HealthSystem>().ResetStatus();
        }).
        AppendInterval(1.3f).
        AppendCallback(() => 
        {
            UIController.Instance.ShowCurrentLevel("G");
            player.GetComponent<PlayerInputController>().canInput = true;
        });
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        AudioManager.Instance.PlayClickSfx();
        player.GetComponent<PlayerInputController>().canInput = true;
        LevelManager.instance.ResetLevel();
        inGameLobby.SetActive(false);
        inGameMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);

        player.GetComponent<Character_HealthSystem>().ResetStatus();
        player.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioManager.Instance.PlayClickSfx();
        pauseMenu.SetActive(false);
        player.GetComponent<PlayerInputController>().canInput = true;
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        player.GetComponent<PlayerInputController>().canInput = false;
    }
}
