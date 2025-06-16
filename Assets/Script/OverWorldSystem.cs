using UnityEngine;
using UnityEngine.SceneManagement;

public class OverWorldSystem : MonoBehaviour
{
    public static bool isInTransition = false;

    [SerializeField]
    private AudioClip overWorldTheme;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isInTransition = false;
        
        int gameProgress = GameData.Instance.GameProgress;

        if (gameProgress >= 5)
        {
            SceneManager.LoadScene("End");
        }

        SoundManager.Instance.PlayMusic(overWorldTheme, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !isInTransition)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
