using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public TMP_Text newGameText;    // selected index = 1
    public TMP_Text loadGameText;   // selected index = 2
    public TMP_Text settingText;    // selected index = 3
    public TMP_Text howToPlay;      // selected index = 4
    public TMP_Text credits;        // selected index = 5
    public TMP_Text exitText;       // selected index = 6

    public GameObject settingWindow;

    public Animator transition;
    public float transitionTime = 1f;

    private bool hasSaveFile;
    private int selectedIndex = 1;
    private bool isInSettingState = false;

    [SerializeField]
    private AudioClip mainTheme;

    [SerializeField]
    private AudioClip moveSFX;

    [SerializeField]
    private AudioClip enterSFX;

    void Awake()
    {
        hasSaveFile = SaveLoadManager.IsSaveFileExist();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hasSaveFile)
        {
            selectedIndex = 2;
        }

        SetTextsColor();
        SoundManager.Instance.PlayMusic(mainTheme, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isInSettingState)
        {
            if (Input.GetKeyDown(KeyCode.Escape) 
                || Input.GetKeyDown(KeyCode.Return) 
                || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                isInSettingState = false;
                settingWindow.SetActive(false);

                SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);
            }
        }
        else
        {
            // enter by selected texts
            if (Input.anyKeyDown
                && !Input.GetKeyDown(KeyCode.DownArrow)
                && !Input.GetKeyDown(KeyCode.UpArrow)
                && !Input.GetKeyDown(KeyCode.Escape)
                && !Input.GetKeyDown(KeyCode.LeftArrow)
                && !Input.GetKeyDown(KeyCode.RightArrow))
            {
                HandleEnter();
            }

            // move selecter
            if (Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetAxis("Mouse ScrollWheel") < 0f) // scroll mouse backward
            {
                IncreaseSelectedIndex();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetAxis("Mouse ScrollWheel") > 0f) // scroll mouse forward
            {
                DecreaseSelectedIndex();
            }
        }
        

    }

    void SetTextsColor()
    {
        // new game
        if (selectedIndex == 1)
        {
            newGameText.color = Color.yellow;
        } 
        else
        {
            newGameText.color = Color.white;
        }

        // load game
        if (!hasSaveFile)
        {
            loadGameText.color = Color.gray;
        } 
        else if (selectedIndex == 2) 
        {
            loadGameText.color = Color.yellow;
        }
        else
        {
            loadGameText.color = Color.white;
        }

        // setting
        if (selectedIndex == 3)
        {
            settingText.color = Color.yellow;
        } 
        else
        {
            settingText.color = Color.white;
        }

        // how to play
        if(selectedIndex == 4)
        {
            howToPlay.color = Color.yellow;
        }
        else
        {
            howToPlay.color = Color.white;
        }

        // credits
        if (selectedIndex == 5)
        {
            credits.color = Color.yellow;
        }
        else
        {
            credits.color = Color.white;
        }

        // exit
        if (selectedIndex == 6)
        {
            exitText.color = Color.yellow;
        }
        else
        {
            exitText.color = Color.white;
        }
    }

    void IncreaseSelectedIndex()
    {
        if (selectedIndex == 1 && !hasSaveFile)
        {
            selectedIndex = 3;
            SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
        } 
        else if (selectedIndex < 6)
        {
            selectedIndex++;
            SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
        }
        SetTextsColor();
    }

    void DecreaseSelectedIndex()
    {
        if (selectedIndex == 3 && !hasSaveFile)
        {
            selectedIndex = 1;
            SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
        } 
        else if (selectedIndex > 1)
        {
            selectedIndex--;
            SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
        }
        SetTextsColor();
    }

    void HandleEnter()
    {
        switch (selectedIndex)
        {
            case 1:
                NewGame();
                SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);
                break;
            case 2:
                LoadGame();
                SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);
                break;
            case 3:
                Setting();
                SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);
                break;
            case 4:
                HowToPlay();
                SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);
                break;
            case 5:
                Credits();
                SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);
                break;
            case 6:
                Exit();
                break;
            default:
                break;
        }
    }

    void NewGame()
    {
        Debug.Log("new game !");

        if (hasSaveFile)
        {
            GameData.Instance.ResetGameData();
        }
        else
        {
            GameData.Instance.SetUpDefault();
        }

        StartCoroutine(ChangeScene("OverWorld"));
    }

    void LoadGame()
    {
        if (!hasSaveFile)
        {
            Debug.Log("You haven't save file!");
            return;
        }

        GameData.Instance.LoadGame();

        Debug.Log("load game !");

        StartCoroutine(ChangeScene("OverWorld"));
    }

    void Setting()
    {
        Debug.Log("setting !");
        isInSettingState = true;
        settingWindow.SetActive(true);
    }

    void HowToPlay()
    {
        Debug.Log("how to play !");

        string fildName = "Manual_Chromagic SP.pdf";
        string filePath = Path.Combine(Application.streamingAssetsPath, fildName);

        Application.OpenURL(filePath);
    }
    void Credits()
    {
        Debug.Log("credits !");

        StartCoroutine(ChangeScene("End"));
    }

    void Exit()
    {
        Debug.Log("exit game !");
        Application.Quit();
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    public void HandleHoverNewGame()
    {
        if (isInSettingState)
        {
            return;
        }
        
        Debug.Log("hover new game");
        selectedIndex = 1;
        SetTextsColor();

        SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
    }

    public void HandleHoverLoadGame()
    {
        if (isInSettingState)
        {
            return;
        }

        Debug.Log("hover load game");

        if (hasSaveFile)
        {
            selectedIndex = 2;
            SetTextsColor();

            SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
        } 
    }

    public void HandleHoverSetting()
    {
        if (isInSettingState)
        {
            return;
        }

        Debug.Log("hover setting game");
        selectedIndex = 3;
        SetTextsColor();

        SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
    }

    public void HandleHoverHowToPlay()
    {
        if (isInSettingState)
        {
            return;
        }

        Debug.Log("hover how to play");
        selectedIndex = 4;
        SetTextsColor();

        SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
    }

    public void HandleHoverCredits()
    {
        if (isInSettingState)
        {
            return;
        }

        Debug.Log("hover credits");
        selectedIndex = 5;
        SetTextsColor();

        SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
    }

    public void HandleHoverExit()
    {
        if (isInSettingState)
        {
            return;
        }

        Debug.Log("hover exit game");
        selectedIndex = 6;
        SetTextsColor();

        SoundManager.Instance.PlaySoundFXClip(moveSFX, 1f, transform);
    }
}
