using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{

    public GameObject clearGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int gameProgress = GameData.Instance.GameProgress;

        if (gameProgress >= 5)
        {
            clearGame.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
